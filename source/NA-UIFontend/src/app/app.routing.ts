import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [
  { path: '', loadChildren: './layout/layout.module#LayoutModule'},
  { path: 'auth', loadChildren: './auth/auth.module#AuthModule'},
  { path: '**', redirectTo: ''},
];

export const AppRoutes = RouterModule.forRoot(routes);
