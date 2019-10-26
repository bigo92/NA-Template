import { Component, OnInit, AfterViewInit, OnChanges, Input, Output, EventEmitter, ElementRef, SimpleChanges, ViewEncapsulation, forwardRef } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

declare let $;
@Component({
  selector: 'input-month',
  templateUrl: './input-month.component.html',
  styleUrls: ['./input-month.component.scss'],
  encapsulation: ViewEncapsulation.None,
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => InputMonthComponent),
      multi: true,
    }
  ]
})
export class InputMonthComponent implements OnInit, AfterViewInit, ControlValueAccessor, OnChanges {

  @Input() class: any = '';
  @Input() placeholder: any = '';
  @Input() disabled: boolean = false;
  @Input() hidden: boolean = false;
  @Input() readonly: boolean = false;
  @Input() format: string = 'MM/yyyy';
  @Input() allowClear: boolean = true;
  @Input() min: number;
  @Input() max: number;
  @Output('onChange') eventOnChange = new EventEmitter<any>();
  @Output('onBlur') eventOnBlur = new EventEmitter<void>();
  @Output('onUnBlur') eventOnUnBlur = new EventEmitter<void>();
  eventBaseChange = (_: any) => { };
  eventBaseTouched = () => { };

  public controlValue: Date | null = null;
  private isFocus: boolean;
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
    this.isFocus = true;
    this.eventBaseTouched();
    this.eventOnBlur.emit();
  }

  onUnBlur() {
    setTimeout(() => {
      this.isFocus = false;
      this.eventOnUnBlur.emit();
    }, 300);
  }

  onChange() {
    if (!this.isFocus) return;

    this.eventBaseChange(this.controlValue);
    this.eventOnChange.emit(this.controlValue);
  }

  setDisabledState?(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }
}
