import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HideColumnPipe } from './hideColumn.pipe';

@NgModule({
    imports: [
        CommonModule
    ],
    exports: [
        HideColumnPipe
    ],
    declarations: [
        HideColumnPipe
    ]
})
export class HideColumnModule { }
