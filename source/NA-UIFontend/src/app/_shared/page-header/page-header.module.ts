import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PageHeaderComponent } from './page-header.component';

@NgModule({
  imports: [
    CommonModule
  ],
  exports:[
    PageHeaderComponent
  ],
  declarations: [PageHeaderComponent]
})
export class PageHeaderModule { }
