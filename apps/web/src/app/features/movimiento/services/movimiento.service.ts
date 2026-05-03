import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import {
  Movimiento,
  MovimientoCreate,
  MovimientoPorCuenta,
  MovimientoUpdate,
  TransferenciaCreate
} from '../models/movimiento.model';

@Injectable({
  providedIn: 'root'
})
export class MovimientoService {
  private readonly baseUrl = `/api/movimientos`;

  constructor(private readonly http: HttpClient) {}

  getMovimientos(): Observable<Movimiento[]> {
    return this.http.get<Movimiento[]>(this.baseUrl);
  }

  getById(idMovimiento: number): Observable<Movimiento> {
    return this.http.get<Movimiento>(`${this.baseUrl}/${idMovimiento}`);
  }

  getByCuenta(idCuenta: number): Observable<MovimientoPorCuenta[]> {
    return this.http.get<MovimientoPorCuenta[]>(`${this.baseUrl}/cuenta/${idCuenta}`);
  }

  create(data: MovimientoCreate): Observable<{ mensaje: string; idMovimiento: number }> {
    return this.http.post<{ mensaje: string; idMovimiento: number }>(this.baseUrl, data);
  }

  update(idMovimiento: number, data: MovimientoUpdate): Observable<{ mensaje: string }> {
    return this.http.put<{ mensaje: string }>(`${this.baseUrl}/${idMovimiento}`, data);
  }

  transferir(data: TransferenciaCreate): Observable<{ mensaje: string }> {
    return this.http.post<{ mensaje: string}>(`${this.baseUrl}/transferencia`, data);
  }
}