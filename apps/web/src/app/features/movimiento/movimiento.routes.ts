import { Routes } from '@angular/router';

import { MovimientoCuentaListComponent } from './pages/movimiento-cuenta-list/movimiento-cuenta-list.component';
import { MovimientoFormComponent } from './pages/movimiento-form/movimiento-form.component';
import { TransferenciaFormComponent } from './pages/transferencia-form/transferencia-form.component';

export const MOVIMIENTO_ROUTES: Routes = [
  {
    path: '',
    component: MovimientoCuentaListComponent
  },
  {
    path: 'crear',
    component: MovimientoFormComponent
  },
  {
    path: 'crear/:idCuenta',
    component: MovimientoFormComponent
  },
  {
    path: 'editar/:idMovimiento',
    component: MovimientoFormComponent
  },
  {
    path: 'cuenta/:idCuenta',
    component: MovimientoCuentaListComponent
  },
  {
    path: 'transferencia/:idCuenta',
    component: TransferenciaFormComponent
  }
];