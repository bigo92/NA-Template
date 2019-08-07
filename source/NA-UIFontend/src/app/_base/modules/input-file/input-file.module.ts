import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { InputFileComponent } from './input-file.component';

@NgModule({
  imports: [
    CommonModule
  ],
  exports:[
    InputFileComponent
  ],
  declarations: [InputFileComponent]
})
export class InputFileModule { }
