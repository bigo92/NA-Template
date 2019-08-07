import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Number2Pipe } from './number2.pipe';

@NgModule({
  imports: [
    CommonModule
  ],
  exports: [
    Number2Pipe
  ],
  declarations: [
    Number2Pipe
  ]
})
export class Number2Module { }