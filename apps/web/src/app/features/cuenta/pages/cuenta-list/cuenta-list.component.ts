import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';

import { CuentaService } from '../../services/cuenta.service';
import { Cuenta, CuentaSaldo } from '../../models/cuenta.model';

@Component({
  selector: 'app-cuenta-list',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './cuenta-list.component.html'
})
export class CuentaListComponent implements OnInit {
  cuentas: Cuenta[] = [];
  saldos: CuentaSaldo[] = [];

  cargando = false;
  error = '';

  constructor(private readonly cuentaService: CuentaService) {}

  ngOnInit(): void {
    this.cargarCuentas();
    this.cargarSaldos();
  }

  cargarCuentas(): void {
    this.cargando = true;
    this.error = '';

    this.cuentaService.getCuentas().subscribe({
      next: (data) => {
        this.cuentas = data;
        this.cargando = false;
      },
      error: (err) => {
        console.error(err);
        this.error = 'No se pudieron cargar las cuentas.';
        this.cargando = false;
      }
    });
  }

  cargarSaldos(): void {
    this.cuentaService.consultarSaldos().subscribe({
      next: (data) => {
        this.saldos = data;
      },
      error: (err) => {
        console.error(err);
      }
    });
  }

  obtenerSaldo(idCuenta: number): number {
    const saldo = this.saldos.find(x => x.idCuenta === idCuenta);
    return saldo?.saldoActual ?? 0;
  }

  cambiarEstado(cuenta: Cuenta): void {
    const peticion = cuenta.activa
      ? this.cuentaService.desactivarCuenta(cuenta.idCuenta)
      : this.cuentaService.activarCuenta(cuenta.idCuenta);

    peticion.subscribe({
      next: () => {
        this.cargarCuentas();
      },
      error: (err) => {
        console.error(err);
        this.error = 'No se pudo cambiar el estado de la cuenta.';
      }
    });
  }

  eliminarCuenta(idCuenta: number): void {
    const confirmar = confirm('¿Seguro que deseas eliminar esta cuenta?');

    if (!confirmar) return;
  }
}