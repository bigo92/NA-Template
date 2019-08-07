import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ViewFilePipe } from './viewFile.pipe';

@NgModule({
  imports: [
    CommonModule
  ],
  exports: [ 
    ViewFilePipe
  ],
  declarations: [ViewFilePipe]
})
export class ViewFileModule { }
