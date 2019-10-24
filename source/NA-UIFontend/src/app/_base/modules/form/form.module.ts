import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { PagingModule } from '../paging/paging.module';
import { RenderErrorModule } from '../render-error/render-error.module';
import { InputTextModule } from '../input-text/input-text.module';
import { InputTextSearchModule } from '../input-text-search/input-text-search.module';
import { InputIntModule } from '../input-int/input-int.module';

@NgModule({
  imports: [
    FormsModule,
    CommonModule,
    RenderErrorModule,
    TranslateModule,
    ReactiveFormsModule,
    PagingModule,
    InputTextModule,
    InputTextSearchModule,
    InputIntModule
  ],
  exports: [
    FormsModule,
    CommonModule,
    RenderErrorModule,
    TranslateModule,
    ReactiveFormsModule,
    PagingModule,
    InputTextModule,
    InputTextSearchModule,
    InputIntModule
  ]
})
export class FormModule {
}
