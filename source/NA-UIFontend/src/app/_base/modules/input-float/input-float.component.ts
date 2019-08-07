import { Component, OnInit, ElementRef, EventEmitter, Output, Input, AfterViewInit, OnChanges, ViewEncapsulation, forwardRef, SimpleChanges } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import createNumberMask from 'text-mask-addons/dist/createNumberMask';

declare var $;
@Component({
  selector: 'input-float',
  templateUrl: './input-float.component.html',
  styleUrls: ['./input-float.component.scss'],
  encapsulation: ViewEncapsulation.None,
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => InputFloatComponent),
      multi: true,
    }]
})
export class InputFloatComponent implements OnInit, AfterViewInit, ControlValueAccessor, OnChanges {

  public controlView: any = '';
  public controlValue: any = '';
  @Input() value: number;
  @Input() max: number;
  @Input() class: any = '';
  @Input() placeholder: any = '';
  @Input() disabled: boolean = false;
  @Input() hidden: boolean = false;
  @Input() allowNegative: boolean = true;
  @Input() min: number;
  @Input() decimalLimit: number = 2;
  public roundNum: number;
  @Output() onChange = new EventEmitter();

  public maskFormat: any;
  constructor(
    private el: ElementRef
  ) {
    this.roundNum = Math.pow(10, this.decimalLimit);
  }

  propagateChange = (_: any) => { };
  onTouched = () => { };

  ngOnInit() {
    setTimeout(() => {
      $(this.el.nativeElement).removeClass(this.class);
    }, 100);

    this.maskFormat = createNumberMask({
      prefix: '',
      allowNegative: this.allowNegative,
      allowDecimal: true,
      decimalLimit: this.decimalLimit,
      decimalSymbol: ',',
      thousandsSeparatorSymbol: ' '
    });
  }

  ngAfterViewInit() {
    //$(this.el.nativeElement).removeClass(this.class);
  }

  ngOnChanges(changes: SimpleChanges) {    
    if (changes.value) { //change attr value
      let obj = changes.value.currentValue;
      if (this.max !== undefined && obj > this.max) {
        obj = this.max;
        setTimeout(() => {
          this.propagateChange(obj);
          this.onChange.emit(obj);
        }, 100);
      }
      this.controlValue = (obj && obj !== 0) ? Math.round(obj * this.roundNum) / this.roundNum : null;
    }
  }

  change() {
    let value = this.controlValue.replace(/\s/g, '');
    value = value.replace(/,/g, '.');
    value = parseFloat(value);
    if(isNaN(value)) value = 0;
    if (this.max !== undefined && value > this.max) {
      value = this.max;
      this.controlValue = value;
    }
    if (this.min !== undefined && value < this.min) {
      value = this.min;
    }
    //value = (value && value !== 0) ? parseFloat(value) : null;
    this.propagateChange(value); // update value to form
    this.onChange.emit(value);
  }

  onBlur() {
    this.onTouched();
  }

  writeValue(obj: any) { //setvaue from control
    let failRanger = false;
    if (this.max !== undefined && obj > this.max) {
      obj = this.max;failRanger = true;
    }
    if (this.min !== undefined && obj < this.min) {
      obj = this.max;failRanger = true;   
    }
    if (failRanger) {
      setTimeout(() => {
        this.propagateChange(obj);
        this.onChange.emit(obj);
      }, 100);
    }
    this.controlValue = (obj && obj !== 0) ? Math.round(obj * this.roundNum) / this.roundNum : null;
  }

  registerOnChange(fn: any) {
    this.propagateChange = fn;
  }

  registerOnTouched(fn: any) {
    this.onTouched = fn;
  }

  setDisabledState?(isDisabled: boolean) {
    this.disabled = isDisabled;
  }
}
