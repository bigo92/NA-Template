import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './home.component';
import { HomeRoutes } from './home.routing';
import { NzIconModule, NzModalModule } from 'ng-zorro-antd';
import { FormModule } from 'src/app/_base/modules/form/form.module';
import { HomeDialogModule } from './home-dialog/home-dialog.module';

@NgModule({
  imports: [
    CommonModule,
    NzIconModule,
    FormModule,
    NzModalModule,
    HomeDialogModule,
    HomeRoutes
  ],
  declarations: [HomeComponent]
})
export class HomeModule { }
