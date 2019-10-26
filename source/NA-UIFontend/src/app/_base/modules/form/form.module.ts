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
import { InputMonthModule } from '../input-month/input-month.module';
import { InputYearModule } from '../input-year/input-year.module';
import { InputDateTimeModule } from '../input-date-time/input-date-time.module';

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
    InputDateModule,
    InputDateTimeModule,
    InputMonthModule,
    InputYearModule
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
    InputDateModule,
    InputDateTimeModule,
    InputMonthModule,
    InputYearModule
  ]
})
export class FormModule {
}
