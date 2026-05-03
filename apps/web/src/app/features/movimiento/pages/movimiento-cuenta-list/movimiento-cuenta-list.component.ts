import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';

import { MovimientoService } from '../../services/movimiento.service';
import { CuentaService } from '../../../cuenta/services/cuenta.service';
import { Movimiento, MovimientoPorCuenta } from '../../models/movimiento.model';

@Component({
  selector: 'app-movimiento-list',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './movimiento-cuenta-list.component.html',
})
export class MovimientoCuentaListComponent implements OnInit {
  movimientos: Array<Movimiento | MovimientoPorCuenta> = [];

  idCuenta: number | null = null;
  nombreCuenta: string | null = null;

  cargando = false;
  error = '';

  constructor(
    private readonly movimientoService: MovimientoService,
    private readonly cuentaService: CuentaService,
    private readonly route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    const idCuentaParam = this.route.snapshot.paramMap.get('idCuenta');

    if (idCuentaParam) {
      this.idCuenta = Number(idCuentaParam);
      this.cargarPorCuenta(this.idCuenta);
      return;
    }

    this.cargarTodos();
  }

  cargarTodos(): void {
    this.cargando = true;
    this.error = '';

    this.movimientoService.getMovimientos().subscribe({
      next: (data) => {
        this.movimientos = data;
        this.cargando = false;
      },
      error: (err) => {
        console.error(err);
        this.error = 'No se pudieron cargar los movimientos.';
        this.cargando = false;
      }
    });
  }

  cargarPorCuenta(idCuenta: number): void {
    this.cargando = true;
    this.error = '';

    this.cuentaService.getCuentaById(idCuenta).subscribe({
      next:(data) => {
        this.nombreCuenta = data.nombre;
      }
    });

    this.movimientoService.getByCuenta(idCuenta).subscribe({
      next: (data) => {
        this.movimientos = data;
        this.cargando = false;
      },
      error: (err) => {
        console.error(err);
        this.error = 'No se pudieron cargar los movimientos de la cuenta.';
        this.cargando = false;
      }
    });
  }

  eliminarMovimiento(idMovimiento: number): void {
    const confirmar = confirm('¿Seguro que deseas eliminar este movimiento?');
    if (!confirmar) return;
  }

  get totalAcreditado(): number {
    return this.movimientos
      .filter(x => x.tipo === 'acreditar')
      .reduce((total, item) => total + Number(item.monto), 0);
  }

  get totalDebitado(): number {
    return this.movimientos
      .filter(x => x.tipo === 'debitar')
      .reduce((total, item) => total + Number(item.monto), 0);
  }

  get saldo(): number {
    return this.totalAcreditado - this.totalDebitado;
  }
}