import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'cuentas',
    pathMatch: 'full'
  },
  {
    path: 'cuentas',
    loadChildren: () =>
      import('./features/cuenta/cuenta.routes').then(m => m.CUENTA_ROUTES)
  },
  {
    path: 'movimientos',
    loadChildren: () =>
      import('./features/movimiento/movimiento.routes').then(m => m.MOVIMIENTO_ROUTES)
  },
  {
    path: '**',
    redirectTo: 'cuentas'
  }
];