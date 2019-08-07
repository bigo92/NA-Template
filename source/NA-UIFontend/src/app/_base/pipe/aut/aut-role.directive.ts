import { Directive, ElementRef, Renderer2 } from '@angular/core';
import { AutService } from './aut.service';

@Directive({
    selector: '[autRole]'
})
export class RoleDirective {
    private option: any;
    constructor(
        private renderer: Renderer2,
        private el: ElementRef,
        private sv: AutService
    ) {
        this.onInit();
    }

    async onInit() {        
        this.option = this.el.nativeElement.getAttribute('option');
        if (this.option === null) {
            this.option = 'none';
        }
        this.sv.getData().subscribe(x => {
            this.renderer.addClass(this.el.nativeElement, 'aut-begin');
            if (x === null) return;
            let autByClaim = this.el.nativeElement.getAttribute('autByClaim') || false;
            let lstRole = [];
            if (autByClaim) {
                lstRole = x.lstClaim;
            } else {
                let autById = this.el.nativeElement.getAttribute('autById') || false;
                let key = autById ? 'roleId' : 'normalizedName';
                lstRole = x.lstRole.map(y => y[key].toString());
            }
            this.checkAut(lstRole);
        });
    }

    async checkAut(roles: any[]) {
        let link = this.el.nativeElement.getAttribute('autRole');
        let isAut = this.el.nativeElement.getAttribute('isAut') || true;
        let lstRole = link.split(',');
        if (roles.length > 0) {
            let isHide = true;
            lstRole.forEach(x => {
                if (roles.includes(x) === JSON.parse(isAut)) {
                    isHide = false;
                }
            });            
            if (isHide) {
                if (this.option === 'none') {
                    this.renderer.setStyle(this.el.nativeElement, 'display', 'none');
                }
                else if(this.option === 'opacity'){
                    this.renderer.setStyle(this.el.nativeElement, 'opacity', 0);
                    this.renderer.setStyle(this.el.nativeElement, 'pointer-events', 'none');                    
                }
                else {
                    this.renderer.setAttribute(this.el.nativeElement,'disabled','disabled');
                    //this.renderer.addClass(this.el.nativeElement, 'disabled');
                    this.renderer.setStyle(this.el.nativeElement, 'pointer-events', 'none');
                }
            }
        }
        this.renderer.removeClass(this.el.nativeElement, 'aut-begin');
    }

}
