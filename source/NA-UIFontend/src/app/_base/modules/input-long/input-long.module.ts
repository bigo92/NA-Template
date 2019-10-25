import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { InputLongComponent } from './input-long.component';
import { FormsModule } from '@angular/forms';
import { NzInputNumberModule } from 'ng-zorro-antd';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    NzInputNumberModule
  ],
  exports:[
    InputLongComponent
  ],
  declarations: [InputLongComponent]
})
export class InputLongModule { }
