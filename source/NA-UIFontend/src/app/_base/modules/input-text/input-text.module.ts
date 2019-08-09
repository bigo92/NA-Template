import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { InputTextComponent } from './input-text.component';
import { NzInputModule } from 'ng-zorro-antd/input';

@NgModule({
  imports: [
    CommonModule,
    NzInputModule
  ],
  exports:[
    InputTextComponent
  ],
  declarations: [InputTextComponent]
})
export class InputTextModule { }
