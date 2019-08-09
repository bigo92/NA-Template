import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeDialogComponent } from './home-dialog.component';
import { NzButtonModule } from 'ng-zorro-antd';

@NgModule({
  imports: [
    CommonModule,
    NzButtonModule
  ],
  exports:[
    HomeDialogComponent
  ],
  declarations: [HomeDialogComponent]
})
export class HomeDialogModule { }
