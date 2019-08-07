import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { InputImgComponent } from './input-img.component';
import { SortablejsModule } from 'angular-sortablejs';
import { LightboxModule } from 'angular2-lightbox';

@NgModule({
  imports: [
    CommonModule,
    LightboxModule,
    SortablejsModule
  ],
  exports:[
    InputImgComponent
  ],
  declarations: [InputImgComponent]
})
export class InputImgModule { }
