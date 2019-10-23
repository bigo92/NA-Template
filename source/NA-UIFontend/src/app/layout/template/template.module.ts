import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TemplateComponent } from './template.component';
import { TemplateRoutes } from './template.routing';
import { NzIconModule, NzModalModule } from 'ng-zorro-antd';
import { FormModule } from 'src/app/_base/modules/form/form.module';
import { TemplateDialogModule } from './template-dialog/template-dialog.module';

@NgModule({
  imports: [
    CommonModule,
    NzIconModule,
    FormModule,
    NzModalModule,
    TemplateDialogModule,
    TemplateRoutes
  ],
  declarations: [TemplateComponent]
})
export class TemplateModule { }
