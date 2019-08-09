import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';

@Component({
  selector: 'app-home-dialog',
  templateUrl: './home-dialog.component.html',
  styleUrls: ['./home-dialog.component.scss']
})
export class HomeDialogComponent implements OnInit {

  @Input('params') params: any;
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
