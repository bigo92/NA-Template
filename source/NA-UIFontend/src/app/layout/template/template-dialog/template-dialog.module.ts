import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TemplateDialogComponent } from './template-dialog.component';
import { NzButtonModule } from 'ng-zorro-antd';
@NgModule({
  imports: [
    CommonModule,
    NzButtonModule
  ],
  exports:[
    TemplateDialogComponent
  ],
  declarations: [TemplateDialogComponent]
})
export class TemplateDialogModule { }
