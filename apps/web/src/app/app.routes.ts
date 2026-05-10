import { Routes } from '@angular/router';

import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'cuentas',
    pathMatch: 'full'
  },
  {
    path: 'auth',
    loadChildren: () =>
      import('./features/auth/auth.routes').then(m => m.AUTH_ROUTES)
  },
  {
    path: 'cuentas',
    canActivate: [authGuard],
    loadChildren: () =>
      import('./features/cuenta/cuenta.routes').then(m => m.CUENTA_ROUTES)
  },
  {
    path: 'movimientos',
    canActivate: [authGuard],
    loadChildren: () =>
      import('./features/movimiento/movimiento.routes').then(m => m.MOVIMIENTO_ROUTES)
  },
  {
    path: '**',
    redirectTo: 'cuentas'
  }
];