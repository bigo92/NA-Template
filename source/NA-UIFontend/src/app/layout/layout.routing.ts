import { Routes, RouterModule } from '@angular/router';
import { LayoutComponent } from './layout.component';

const routes: Routes = [
  { path: '', component: LayoutComponent, children: [
      { path: '', loadChildren: './home/home.module#HomeModule' }
  ]}
];

export const LayoutRoutes = RouterModule.forChild(routes);
