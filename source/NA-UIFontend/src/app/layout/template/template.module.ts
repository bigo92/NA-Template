import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TemplateComponent } from './template.component';
import { TemplateRoutes } from './template.routing';

@NgModule({
  imports: [
    CommonModule,
    TemplateRoutes
  ],
  declarations: [TemplateComponent]
})
export class TemplateModule { }
