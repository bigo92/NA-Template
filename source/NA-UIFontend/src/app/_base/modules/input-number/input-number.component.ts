import { Component, OnInit, ViewEncapsulation, forwardRef, AfterViewInit, OnChanges, Input, Output, EventEmitter, ElementRef, SimpleChanges } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import { InputTextComponent } from '../input-text/input-text.component';
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
      useExisting: forwardRef(() => InputNumberComponent),
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
  @Input() symbol: string = ',';
  @Input() prefix: string = '';
  @Output('onChange') eventOnChange = new EventEmitter<any>();
  @Output('onBlur') eventOnBlur =  new EventEmitter<void>();
  @Output('onUnBlur') eventOnUnBlur =  new EventEmitter<void>();
  eventBaseChange = (_: any) => { };
  eventBaseTouched = () => { };

  public controlValue: any = '';
  public maskFomat = createNumberMask({
    prefix: this.prefix,
    allowNegative: true,
    allowDecimal: false,
    thousandsSeparatorSymbol: this.symbol
  });
  constructor(
    private el: ElementRef
  ) { }

  ngOnInit() {
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

  onUnBlur(){
    this.eventOnUnBlur.emit();
  }

  onChange() {
    let val = this.getValue();
    this.eventBaseChange(+val);
    this.eventOnChange.emit(+val);
  }

  setDisabledState?(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }

  pushValue(){
    let val = this.getValue();
    if (this.max && +val >= this.max) return;
    this.controlValue = +val + this.step;
    this.onChange();
  }

  minusValue(){
    let val = this.getValue();
    if (this.min && +val <= this.min) return;
    this.controlValue = +val - this.step;
    this.onChange();
  }

  private getValue(){
    let val = (this.controlValue+'').replace(new RegExp(this.symbol, 'g'),'');
    if (this.prefix !== '') {
      val = val.replace(new RegExp(this.prefix, 'g'),'.');
    }
    return +val;
  }
}

