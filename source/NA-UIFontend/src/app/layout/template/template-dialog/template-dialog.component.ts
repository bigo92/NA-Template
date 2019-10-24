import { Component, OnInit, Input, Output, EventEmitter, TemplateRef } from '@angular/core';
import { NgTemplateOutlet } from '@angular/common';

@Component({
  selector: 'app-template-dialog',
  templateUrl: './template-dialog.component.html',
  styleUrls: ['./template-dialog.component.scss']
})
export class TemplateDialogComponent implements OnInit {

  @Input('params') params: any;
  @Input('template') template: NgTemplateOutlet;
  @Input('setting') setting: any;
  @Output('nzOnOk') nzOnOk = new EventEmitter<any>();
  @Output('nzOnCancel') nzOnCancel = new EventEmitter<void>();
  isDialogLoading: boolean = false;
  constructor() { }

  ngOnInit() {
    if (this.params.mode === 1) {
      //view
    }
  }

  handleCancel() {
    this.nzOnCancel.emit();
  }

  handleOk() {
    this.isDialogLoading = true;
    setTimeout(() => {
      this.nzOnOk.emit(null);
    }, 3000);
  }

}
