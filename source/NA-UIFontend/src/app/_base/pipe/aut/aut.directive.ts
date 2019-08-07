import { Directive, ElementRef } from '@angular/core';
import { AutService } from './aut.service';

@Directive({
  selector: '[appAut]'
})
export class AutDirective {
  constructor(
    private el: ElementRef,
    private sv: AutService
  ) {
    this.onInit();
  }

  async onInit() {
    this.sv.getData().subscribe(x => {
      this.el.nativeElement.style.display = 'none';
      if (x === null) return;
      this.checkAut(x.lstClaim);
    })
  }

  async checkAut(claims: any[]) {
    var link = this.el.nativeElement.getAttribute('appAut');
    var lstAut = link.split(',');
    if (claims.length > 0) {
      let isHide = true;
      lstAut.forEach(x => {
        if(claims.includes(x)) {
          isHide = false;
        }
      });
      if (!isHide) {
        this.el.nativeElement.style = null;
      }
    }
  }

}
