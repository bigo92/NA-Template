import { Routes, RouterModule } from '@angular/router';
import { LayoutComponent } from './layout.component';

const routes: Routes = [
  {
    path: '', component: LayoutComponent, children: [
      { path: '', loadChildren: () => import('./home/home.module').then(m => m.HomeModule) },
      { path: 'template', loadChildren: () => import('./template/template.module').then(m => m.TemplateModule) }
    ]
  }
];

export const LayoutRoutes = RouterModule.forChild(routes);
