import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { PagingModule } from '../paging/paging.module';
import { RenderErrorModule } from '../render-error/render-error.module';
import { InputTextModule } from '../input-text/input-text.module';
import { InputTextSearchModule } from '../input-text-search/input-text-search.module';
import { InputNumberModule } from '../input-number/input-number.module';
import { InputDateModule } from '../input-date/input-date.module';

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
    InputNumberModule,
    InputDateModule
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
    InputNumberModule,
    InputDateModule
  ]
})
export class FormModule {
}
