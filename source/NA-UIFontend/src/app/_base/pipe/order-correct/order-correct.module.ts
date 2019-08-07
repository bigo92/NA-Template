import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OrderCorrectPipe } from './order-correct.pipe';

@NgModule({
   imports: [
      CommonModule
   ],
   exports: [
      OrderCorrectPipe
   ],
   declarations: [
      OrderCorrectPipe
   ]
})
export class OrderCorrectModule { }
