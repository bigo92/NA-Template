import { Component, OnInit, AfterViewInit } from '@angular/core';
declare var App: any;

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss']
})

export class LayoutComponent implements OnInit, AfterViewInit {

  constructor() { }

  ngOnInit() {
    App.initBeforeLoad();
    window.addEventListener('load', () => {
      App.initAfterLoad();
    });
  }

  ngAfterViewInit() {
    App.initCore();
  }

}
