import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PagingComponent } from './paging.component';

@NgModule({
  imports: [
    CommonModule
  ],
  providers: [PagingComponent],
  exports: [PagingComponent],
  declarations: [PagingComponent]
})
export class PagingModule { }
