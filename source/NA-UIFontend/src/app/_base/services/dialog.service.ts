import { Injectable, TemplateRef } from '@angular/core';
import { BsModalService, BsModalRef } from 'ngx-bootstrap';
@Injectable({
  providedIn: 'root'
})
export class DialogService {
  private bsModalRef: BsModalRef;
  private lstmodalRef: any[] = [];
  private lstConfirmDialog: string[] = [];
  private configDialog = {
    animated: false,
    keyboard: false,
    backdrop: true,
    ignoreBackdropClick: true,
    class: 'modal-dialog-centered'
  };
  constructor(
    private bsModalSv: BsModalService,
  ) { }

  public openModal(template: TemplateRef<any>, isOneDialog: boolean = false) {
    const index = this.lstmodalRef.findIndex(x => x.temp === template);
    if (index === -1) {
      const itemModel = this.bsModalSv.show(template, Object.assign({}, this.configDialog));
      this.lstmodalRef.push({ temp: template, model: itemModel });
    }
    if (isOneDialog) {
      this.lstmodalRef.filter(x => x.temp !== template).forEach(x => {
        this.closeModal(x.temp);
      });
    }
  }


  public closeModal(template: TemplateRef<any> = null) {
    if (template !== null) {
      const index = this.lstmodalRef.findIndex(x => x.temp === template);
      if (index !== -1) {
        this.lstmodalRef[index].model.hide();
        this.lstmodalRef.splice(index, 1);
      }
    } else {
      while (this.lstmodalRef.length > 0) {
        this.lstmodalRef[0].model.hide();
        this.lstmodalRef.splice(0, 1);
      }
    }
  }
}
