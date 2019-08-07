import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OrderCodePipe } from './order-code.pipe';

@NgModule({
   imports: [
      CommonModule
   ],
   exports: [
      OrderCodePipe
   ],
   declarations: [
      OrderCodePipe
   ]
})
export class OrderCodeModule { }
