import { Component, OnInit, ViewEncapsulation, forwardRef, AfterViewInit, OnChanges, Input, Output, EventEmitter, ElementRef, SimpleChanges } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import { InputTextComponent } from '../input-text/input-text.component';
import { DecimalPipe } from '@angular/common';
import createNumberMask from 'text-mask-addons/dist/createNumberMask';

declare var $;
@Component({
  selector: 'input-number',
  templateUrl: './input-number.component.html',
  styleUrls: ['./input-number.component.scss'],
  encapsulation: ViewEncapsulation.None,
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => InputTextComponent),
      multi: true,
    }
  ]
})
export class InputNumberComponent implements OnInit, AfterViewInit, ControlValueAccessor, OnChanges {

  @Input() class: any = '';
  @Input() placeholder: any = '';
  @Input() disabled: boolean = false;
  @Input() hidden: boolean = false;
  @Input() readonly: boolean = false;
  @Input() min: number;
  @Input() max: number;
  @Input() step: number = 1;
  @Output('onChange') eventOnChange = new EventEmitter<any>();
  @Output('onBlur') eventOnBlur =  new EventEmitter<void>();
  eventBaseChange = (_: any) => { };
  eventBaseTouched = () => { };

  public controlValue: any = ''; 
  public maskFomat = createNumberMask({
    prefix: '',
    allowNegative: true,
    allowDecimal: false,
    thousandsSeparatorSymbol: ","
  }); 
  constructor(
    private el: ElementRef,
    private decimalPipe: DecimalPipe
  ) { }

  ngOnInit() {
    let a = this.decimalPipe.transform('1234', '1.0');
  }

  ngOnChanges(changes: SimpleChanges): void {
  }

  ngAfterViewInit() {
    $(this.el.nativeElement).removeClass(this.class);
  }

  writeValue(obj: any) {
    this.controlValue = obj;
  }

  registerOnChange(fn: any) {
    this.eventBaseChange = fn;
  }

  registerOnTouched(fn: any) {
    this.eventBaseTouched = fn;
  }

  onBlur() {
    this.eventBaseTouched();
    this.eventOnBlur.emit();
  }

  onChange() {
    this.eventBaseChange(this.controlValue);
    this.eventOnChange.emit(this.controlValue);
  }

  setDisabledState?(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }

  pushValue(){
    if (this.max && +this.controlValue >= this.max) return;
    this.controlValue++;
  }

  minusValue(){
    if (this.min && +this.controlValue <= this.min) return;
    this.controlValue--;
  }
}

