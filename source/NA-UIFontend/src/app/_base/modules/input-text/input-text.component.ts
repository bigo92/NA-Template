import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'input-text',
  templateUrl: './input-text.component.html',
  styleUrls: ['./input-text.component.scss']
})
export class InputTextComponent implements OnInit {

  @Input() placeholder: string;
  @Input() size = 'default';
  constructor() { }

  ngOnInit() {
  }

  logDelay(value: any) {
    console.log(value);
  }

}
