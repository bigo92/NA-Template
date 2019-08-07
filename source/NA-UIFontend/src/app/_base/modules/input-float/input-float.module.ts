import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { InputFloatComponent } from './input-float.component';
import { FormsModule } from '@angular/forms';
import { TextMaskModule } from '../../libs/angular2TextMask';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    TextMaskModule
  ],
  exports: [
    InputFloatComponent
  ],
  declarations: [InputFloatComponent]
})
export class InputFloatModule { }
