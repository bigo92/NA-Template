import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Date2Pipe } from './date2.pipe';

@NgModule({
  imports: [
    CommonModule
  ],
  exports:[
    Date2Pipe
  ],
  declarations: [Date2Pipe]
})
export class Date2Module { }