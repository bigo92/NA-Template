import { Directive, ElementRef } from '@angular/core';
import { AutService } from './aut.service';

@Directive({
  selector: '[appUAut]'
})
export class UAutDirective {

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
    var link = this.el.nativeElement.getAttribute('appUAut');
    var lstAut = link.split(',');
    if (claims.length > 0) {
      let isHide = false;
      for (const iAut of lstAut) {
        for (const xValue of claims) {
          let v = xValue.split(',');
          if (v.findIndex(x => x === iAut) !== -1) {
            isHide = true;
          }
        }
      }
      if (!isHide) {
        this.el.nativeElement.style = null;
      }
    }
  }

}
