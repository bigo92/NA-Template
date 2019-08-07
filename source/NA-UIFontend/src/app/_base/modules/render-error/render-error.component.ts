import { FormControl } from '@angular/forms';
import { Component, OnInit, Input, OnChanges } from '@angular/core';

@Component({
  selector: 'render-error',
  templateUrl: './render-error.component.html',
  styleUrls: ['./render-error.component.scss']
})
export class RenderErrorComponent implements OnInit, OnChanges {
  @Input('data') myControl: FormControl;
  @Input() className = '';
  ngClass: any = {};
  constructor(
  ) { }

  ngOnInit() {
    this.changeToArray(this.myControl);
    this.ngClass[this.className] = this.className ? true : false;
  }

  ngOnChanges() {
    // console.log(this.myControl.errors);
  }

  public lstError = [];
  public error = {};
  changeToArray(obj: any) {
    if (obj.errors == this.error) {
      return this.lstError;
    }

    this.lstError = [];
    this.error = obj.errors;
    if (obj.errors != null) {
      this.lstError = [];
      for (var key in obj.errors) {
        let jsonData = `{"key":"${key}","value": "${obj.errors[key]}"}`;
        let data = JSON.parse(jsonData);
        this.lstError.push(data);
      }
    }
    return this.lstError;
  }
}
