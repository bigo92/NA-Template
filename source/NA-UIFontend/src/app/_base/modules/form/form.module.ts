import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { PagingModule } from '../paging/paging.module';
import { RenderErrorModule } from '../render-error/render-error.module';
import { NumToWords } from '../../pipe/money/money-words.pipe';
import { MoneyFormat } from '../../pipe/money/money-format.pipe';
import { DateFormat } from '../../pipe/date/date-format.pipe';

@NgModule({
  imports: [
    FormsModule,
    CommonModule,
    RenderErrorModule,
    TranslateModule,
    ReactiveFormsModule,
    PagingModule
  ],
  exports: [
    FormsModule,
    CommonModule,
    RenderErrorModule,
    TranslateModule,
    ReactiveFormsModule,
    PagingModule
  ],
  declarations: [NumToWords, MoneyFormat, DateFormat]
})
export class FormModule {
}
