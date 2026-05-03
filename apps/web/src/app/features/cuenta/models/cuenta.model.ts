export interface Cuenta {
  idCuenta: number;
  nombre: string;
  tipo: string;
  activa: boolean;
  created: string;
  updated: string;
}

export interface CuentaCreate {
  nombre: string;
  tipo: string;
  activa: boolean;
}

export interface CuentaUpdate {
  nombre: string;
  tipo: string;
  activa: boolean;
}

export interface CuentaSaldo {
  idCuenta: number;
  nombre: string;
  saldoActual: number;
}