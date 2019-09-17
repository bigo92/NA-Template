import { Directive, Output, EventEmitter, HostListener, ElementRef, Input } from '@angular/core';

@Directive({
  selector: '[appInputText]'
})
export class InputTextDirective {

  @Output('onDelay') onDelay = new EventEmitter<any>(null);
  timeout: any;
  @Input() delay = 1000;
  constructor(
    private el: ElementRef
  ) { }

  @HostListener('keyup') onChange() {
    clearTimeout(this.timeout);
    this.timeout = setTimeout(() => {
      this.onDelay.emit(this.el.nativeElement.value);
    }, this.delay);
  }

}
