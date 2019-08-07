import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { WhereColumnPipe } from './whereColumn.pipe';

@NgModule({
   imports: [
      CommonModule
   ],
   exports:[
    WhereColumnPipe
   ],
   declarations: [
      WhereColumnPipe
   ]
})
export class WhereColumnModule { }
