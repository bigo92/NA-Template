import { Component, OnInit, AfterViewInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit, AfterViewInit {

  paging: any ={
    page: 1,
    count: 100,
    totalPage: 50,
    size: 20
  }
  showDialog: boolean = false;
  paramsDialog: any = {};
  isDialogLoading: boolean = false;
  constructor() { }

  ngOnInit() {
  }

  ngAfterViewInit() {
    console.log('ngAfterViewInit home');
  }

  createDialog(){
    this.paramsDialog.mode = 1;
    this.paramsDialog.id = null;
    this.showDialog = true;
  }

  editDialog(item: any){
    this.paramsDialog.mode = 2;
    this.paramsDialog.id = item.id;
    this.showDialog = true;
  }

  viewDialog(item: any){
    this.paramsDialog.mode = 3;
    this.paramsDialog.id = item.id;
    this.showDialog = true;
  }

  handleCancel(): void {
    this.showDialog = false;
  }

  handleOk(): void {
    this.showDialog = false;
  }
}
