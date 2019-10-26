import { Component, OnInit, ViewEncapsulation, forwardRef, AfterViewInit, OnChanges, Input, Output, EventEmitter, ElementRef, SimpleChanges } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';

declare let $;
@Component({
  selector: 'input-date-time',
  templateUrl: './input-date-time.component.html',
  styleUrls: ['./input-date-time.component.scss'],
  encapsulation: ViewEncapsulation.None,
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => InputDateTimeComponent),
      multi: true,
    }
  ]
})
export class InputDateTimeComponent implements OnInit, AfterViewInit, ControlValueAccessor, OnChanges {

  @Input() class: any = '';
  @Input() placeholder: any = '';
  @Input() disabled: boolean = false;
  @Input() hidden: boolean = false;
  @Input() readonly: boolean = false;
  @Input() format: string = 'dd/MM/yyyy HH:mm:ss';
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
    $(this.el.nativeElement).find('.ant-calendar-picker').css({'width':'100%'})
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

