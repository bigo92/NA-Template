import { Component, OnInit, AfterViewInit } from '@angular/core';
declare var App;

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss']
})

export class LayoutComponent implements OnInit, AfterViewInit {

  constructor() { }

  ngOnInit() {
    console.log('ngOnInit laypout');
    App.initBeforeLoad();
    window.addEventListener('load', function () {
      console.log('load laypout');
      App.initAfterLoad();
    });
  }

  ngAfterViewInit() {
    console.log('ngAfterViewInit laypout');
    App.initCore();
  }

}
