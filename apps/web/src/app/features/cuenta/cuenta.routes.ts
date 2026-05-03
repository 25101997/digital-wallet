import { Routes } from '@angular/router';

import { CuentaListComponent } from './pages/cuenta-list/cuenta-list.component';
import { CuentaFormComponent } from './pages/cuenta-form/cuenta-form.component';

export const CUENTA_ROUTES: Routes = [
  {
    path: '',
    component: CuentaListComponent
  },
  {
    path: 'nueva',
    component: CuentaFormComponent
  },
  {
    path: 'editar/:idCuenta',
    component: CuentaFormComponent
  }
];