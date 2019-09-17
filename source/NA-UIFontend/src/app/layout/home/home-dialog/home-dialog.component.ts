import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { BehaviorSubject, Subject } from 'rxjs';

@Component({
  selector: 'app-home-dialog',
  templateUrl: './home-dialog.component.html',
  styleUrls: ['./home-dialog.component.scss']
})
export class HomeDialogComponent implements OnInit {

  @Input() params: any;
  @Output() nzOnOk = new EventEmitter<any>();
  @Output() nzOnCancel = new EventEmitter<void>();
  @Input() onSave: EventEmitter<boolean>;
  isDialogLoading: boolean = false;
  constructor() { }

  ngOnInit() {
    if (this.params.mode === 1) {
      //view
    }
    this.onSave.subscribe(x => console.log(x));
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
