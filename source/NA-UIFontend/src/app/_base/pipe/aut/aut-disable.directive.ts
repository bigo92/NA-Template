import { Directive, ElementRef } from '@angular/core';
import { AutService } from './aut.service';

declare var $;
@Directive({
  selector: '[appAutDisable]'
})
export class AutDisableDirective {

  constructor(
    private el: ElementRef,
    private sv: AutService
  ) {
    this.onInit();
  }

  async onInit() {
    this.sv.getData().subscribe(x=>{
      setTimeout(() => {
        $(this.el.nativeElement.querySelector('select')).prop("disabled", true);
      }, 10);
      if(x === null) return;
      this.checkAut(x.lstClaim);
    })
  }

  async checkAut(claims: any[]){
    var link = this.el.nativeElement.getAttribute('appAutDisable');
    var lstAut = link.split(',');    
    if (claims.length > 0) {
      let isDisabled = true;
      for (const iAut of lstAut) {
        if (claims.findIndex(x=>x === iAut) !== -1) {
          isDisabled = false;
        } 
      }   
      setTimeout(() => {
        $(this.el.nativeElement.querySelector('select')).prop("disabled", isDisabled);
      }, 15);
    }
  }


}
