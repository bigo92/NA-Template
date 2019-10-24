import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { InputIntComponent } from './input-int.component';
import { NzInputNumberModule } from 'ng-zorro-antd';

@NgModule({
  imports: [
    CommonModule,
    NzInputNumberModule
  ],
  exports:[
    InputIntComponent
  ],
  declarations: [InputIntComponent]
})
export class InputIntModule { }
