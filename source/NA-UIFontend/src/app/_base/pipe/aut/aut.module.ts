import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AutDirective } from './aut.directive';
import { AutService } from './aut.service';
import { UAutDirective } from './u-aut.directive';
import { AutDisableDirective } from './aut-disable.directive';
import { RoleDirective } from './aut-role.directive';

@NgModule({
   imports: [
      CommonModule
   ],
   declarations: [
      AutDirective,
      RoleDirective,
      UAutDirective,
      AutDisableDirective
   ],
   exports: [
      AutDirective,
      RoleDirective,
      UAutDirective,
      AutDisableDirective
   ]
})
export class AutModule { }
