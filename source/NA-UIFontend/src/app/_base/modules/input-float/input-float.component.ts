import { Component, OnInit, ViewEncapsulation, forwardRef, AfterViewInit, OnChanges, Input, Output, EventEmitter, ElementRef, SimpleChanges } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import { InputNumberComponent } from '../input-number/input-number.component';
import createNumberMask from 'text-mask-addons/dist/createNumberMask';

declare let $;
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
    }
  ]
})
export class InputFloatComponent implements OnInit, AfterViewInit, ControlValueAccessor, OnChanges {

  @Input() class: any = '';
  @Input() placeholder: any = '';
  @Input() disabled: boolean = false;
  @Input() hidden: boolean = false;
  @Input() readonly: boolean = false;
  @Input() min: number;
  @Input() max: number;
  @Input() step: number = 1;
  @Input() symbol: string = ' ';
  @Input() prefix: string = '';
  @Input() decimalLimit : number = 2;
  @Input() integerLimit : number = null;
  @Output('onChange') eventOnChange = new EventEmitter<any>();
  @Output('onBlur') eventOnBlur =  new EventEmitter<void>();
  @Output('onUnBlur') eventOnUnBlur =  new EventEmitter<void>();
  eventBaseChange = (_: any) => { };
  eventBaseTouched = () => { };

  public controlValue: number | null = null;
  public maskFomat = createNumberMask({
    prefix: this.prefix,
    suffix : '',
    allowNegative: true,
    allowDecimal: true,
    decimalLimit : this.decimalLimit,
    integerLimit : this.integerLimit,
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
    let stepUnit = this.getUnit();
    if (this.max && +val >= this.max) return;
    this.controlValue = +val + (this.step/ stepUnit);
    this.onChange();
  }

  minusValue(){
    let val = this.getValue();
    let stepUnit = this.getUnit();
    if (this.min && +val <= this.min) return;
    this.controlValue = +val - (this.step/ stepUnit);
    this.onChange();
  }

  private getValue(){
    let val: any = this.controlValue;
    if(!val) val = '';
    val = val.toString().replace(new RegExp(this.symbol, 'g'),'');
    if (this.prefix !== '') {
      val = val.replace(new RegExp(this.prefix, 'g'),'');
    }
    return +val;
  }

  private getUnit(){
    let val = (this.controlValue+'').replace(new RegExp(this.symbol, 'g'),'');
    if (this.prefix !== '') {
      val = val.replace(new RegExp(this.prefix, 'g'),'');
    }
    let unit = val.substring(val.indexOf('.'));
    return Math.pow(10,unit.replace('.','').length);
  }
}
