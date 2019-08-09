import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './home.component';
import { HomeRoutes } from './home.routing';
import { NzIconModule } from 'ng-zorro-antd';
import { FormModule } from 'src/app/_base/modules/form/form.module';

@NgModule({
  imports: [
    CommonModule,
    NzIconModule,
    FormModule,
    HomeRoutes
  ],
  declarations: [HomeComponent]
})
export class HomeModule { }
