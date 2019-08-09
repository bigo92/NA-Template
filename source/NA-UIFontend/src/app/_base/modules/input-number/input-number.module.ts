import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { InputNumberComponent } from './input-number.component';
import { NzInputNumberModule } from 'ng-zorro-antd';

@NgModule({
  imports: [
    CommonModule,
    NzInputNumberModule
  ],
  exports:[
    InputNumberComponent
  ],
  declarations: [InputNumberComponent]
})
export class InputNumberModule { }
