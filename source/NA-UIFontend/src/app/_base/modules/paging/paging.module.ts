import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PagingComponent } from './paging.component';
import { NgZorroAntdModule } from 'ng-zorro-antd';

@NgModule({
  imports: [
    CommonModule,
    NgZorroAntdModule
  ],
  providers: [PagingComponent],
  exports: [PagingComponent],
  declarations: [PagingComponent]
})
export class PagingModule { }