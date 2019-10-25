import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { InputIntComponent } from './input-int.component';
import { NzInputNumberModule } from 'ng-zorro-antd/input-number';
import { FormsModule } from '@angular/forms';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    NzInputNumberModule
  ],
  exports:[
    InputIntComponent
  ],
  declarations: [InputIntComponent]
})
export class InputIntModule { }
