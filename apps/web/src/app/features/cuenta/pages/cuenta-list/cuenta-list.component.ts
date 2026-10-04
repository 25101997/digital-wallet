import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';

import { CuentaService } from '../../services/cuenta.service';
import { Cuenta, CuentaSaldo } from '../../models/cuenta.model';

@Component({
  selector: 'app-cuenta-list',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    FormsModule
  ],
  templateUrl: './cuenta-list.component.html',
  styleUrl: './cuenta-list.component.css'
})
export class CuentaListComponent implements OnInit {

  cuentas: Cuenta[] = [];
  saldos: CuentaSaldo[] = [];

  cargando = false;
  error = '';

  // Filtros
  textoBusqueda = '';
  tipoSeleccionado = '';
  estadoSeleccionado = 'activas';
  saldoSeleccionado = '';
  ordenSeleccionado = 'nombre';

  constructor(
    private readonly cuentaService: CuentaService
  ) {}

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
    const saldo = this.saldos.find(
      x => x.idCuenta === idCuenta
    );

    return saldo?.saldoActual ?? 0;
  }

  get tiposCuenta(): string[] {
    const tipos = this.cuentas
      .map(cuenta => cuenta.tipo)
      .filter(tipo => !!tipo);

    return [...new Set(tipos)].sort();
  }

  get cuentasFiltradas(): Cuenta[] {

    let resultado = [...this.cuentas];

    // Buscar por nombre
    if (this.textoBusqueda.trim()) {
      const texto = this.textoBusqueda
        .trim()
        .toLowerCase();

      resultado = resultado.filter(cuenta =>
        cuenta.nombre.toLowerCase().includes(texto)
      );
    }

    // Filtrar por tipo
    if (this.tipoSeleccionado) {
      resultado = resultado.filter(
        cuenta => cuenta.tipo === this.tipoSeleccionado
      );
    }

    // Filtrar por estado
    if (this.estadoSeleccionado === 'activas') {
      resultado = resultado.filter(
        cuenta => cuenta.activa
      );
    }

    if (this.estadoSeleccionado === 'inactivas') {
      resultado = resultado.filter(
        cuenta => !cuenta.activa
      );
    }

    // Filtrar por saldo
    if (this.saldoSeleccionado === 'con-saldo') {
      resultado = resultado.filter(
        cuenta => this.obtenerSaldo(cuenta.idCuenta) !== 0
      );
    }

    if (this.saldoSeleccionado === 'saldo-cero') {
      resultado = resultado.filter(
        cuenta => this.obtenerSaldo(cuenta.idCuenta) === 0
      );
    }

    if (this.saldoSeleccionado === 'positivo') {
      resultado = resultado.filter(
        cuenta => this.obtenerSaldo(cuenta.idCuenta) > 0
      );
    }

    if (this.saldoSeleccionado === 'negativo') {
      resultado = resultado.filter(
        cuenta => this.obtenerSaldo(cuenta.idCuenta) < 0
      );
    }

    // Ordenar
    resultado.sort((a, b) => {

      switch (this.ordenSeleccionado) {

        case 'saldo-mayor':
          return (
            this.obtenerSaldo(b.idCuenta) -
            this.obtenerSaldo(a.idCuenta)
          );

        case 'saldo-menor':
          return (
            this.obtenerSaldo(a.idCuenta) -
            this.obtenerSaldo(b.idCuenta)
          );

        case 'tipo':
          return a.tipo.localeCompare(b.tipo);

        case 'nombre':
        default:
          return a.nombre.localeCompare(b.nombre);
      }
    });

    return resultado;
  }

  get totalFiltrado(): number {
    return this.cuentasFiltradas.reduce(
      (total, cuenta) =>
        total + this.obtenerSaldo(cuenta.idCuenta),
      0
    );
  }

  get totalGeneral(): number {
    return this.cuentas
      .filter(cuenta => cuenta.activa)
      .reduce(
        (total, cuenta) =>
          total + this.obtenerSaldo(cuenta.idCuenta),
        0
      );
  }

  totalPorTipo(tipo: string): number {
    return this.cuentas
      .filter(
        cuenta =>
          cuenta.activa &&
          cuenta.tipo.toLowerCase() === tipo.toLowerCase()
      )
      .reduce(
        (total, cuenta) =>
          total + this.obtenerSaldo(cuenta.idCuenta),
        0
      );
  }

  aplicarFiltroTipo(tipo: string): void {
    this.tipoSeleccionado = tipo;
  }

  limpiarFiltros(): void {
    this.textoBusqueda = '';
    this.tipoSeleccionado = '';
    this.estadoSeleccionado = 'activas';
    this.saldoSeleccionado = '';
    this.ordenSeleccionado = 'nombre';
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
    const confirmar = confirm(
      '¿Seguro que deseas eliminar esta cuenta?'
    );

    if (!confirmar) {
      return;
    }
  }
}