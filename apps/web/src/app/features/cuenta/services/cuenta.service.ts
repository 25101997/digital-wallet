import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Cuenta, CuentaCreate, CuentaSaldo, CuentaUpdate } from '../models/cuenta.model';

@Injectable({
  providedIn: 'root'
})
export class CuentaService {
  private readonly apiUrl = '/api';

  constructor(private http: HttpClient) {}
  
  createCuenta(data: CuentaCreate): Observable<{ mensaje: string; idCuenta: number }> {
    return this.http.post<{ mensaje: string; idCuenta: number }>(`${this.apiUrl}/crear-cuenta`, data);
  }

  getCuentas(): Observable<Cuenta[]> {
    return this.http.get<Cuenta[]>(`${this.apiUrl}/cuentas`);
  }

  getCuentaById(idCuenta: number): Observable<Cuenta> {
    return this.http.get<Cuenta>(`${this.apiUrl}/cuenta/${idCuenta}`);
  }

  consultarSaldos() {
    return this.http.get<CuentaSaldo[]>(`${this.apiUrl}/cuentas-con-saldos`);
  }

  updateCuenta(idCuenta: number, data: CuentaUpdate): Observable<{ mensaje: string }> {
    return this.http.put<{ mensaje: string }>(`${this.apiUrl}/actualizar-cuenta/${idCuenta}`, data);
  }

  activarCuenta(idCuenta: number): Observable<{ mensaje: string }> {
    return this.http.patch<{ mensaje: string }>(`${this.apiUrl}/${idCuenta}/activar`, {});
  }

  desactivarCuenta(idCuenta: number): Observable<{ mensaje: string }> {
    return this.http.patch<{ mensaje: string }>(`${this.apiUrl}/${idCuenta}/desactivar`, {});
  }
}