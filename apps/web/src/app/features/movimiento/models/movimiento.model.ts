export type MovimientoTipo = 'acreditar' | 'debitar';

export type MovimientoVia = 'transferencia' | 'efectivo' | 'deposito' | 'retiro' | 'ajuste';

export interface Movimiento {
  idMovimiento: number;
  idCuenta: number;
  nombreCuenta?: string | null;
  tipo: MovimientoTipo;
  monto: number;
  descripcion?: string | null;
  via: MovimientoVia;
  idCuentaOrigen?: number | null;
  nombreCuentaOrigen?: string | null;
  idCuentaDestino?: number | null;
  nombreCuentaDestino?: string | null;
  created: string;
  updated: string;
}

export interface MovimientoCreate {
  idCuenta: number;
  tipo: MovimientoTipo;
  monto: number;
  descripcion?: string | null;
  via: MovimientoVia;
}

export interface MovimientoUpdate {
  idCuenta: number;
  tipo: MovimientoTipo;
  monto: number;
  descripcion?: string | null;
  via: MovimientoVia;
}

export interface MovimientoPorCuenta {
  idMovimiento: number;
  nombreCuenta: string;
  tipo: MovimientoTipo;
  monto: number;
  descripcion?: string | null;
  via: MovimientoVia;
  idCuentaOrigen?: number | null;
  nombreCuentaOrigen?: string | null;
  idCuentaDestino?: number | null;
  nombreCuentaDestino?: string | null;
  created: string;
  updated: string;
}

export interface TransferenciaCreate {
  idCuenta: number;
  idCuentaDestino: number;
  monto: number;
  descripcion?: string | null;
}