import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';

import { Cuenta } from '../../../cuenta/models/cuenta.model';
import { CuentaService } from '../../../cuenta/services/cuenta.service';
import { MovimientoService } from '../../services/movimiento.service';

@Component({
  selector: 'app-movimiento-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './movimiento-form.component.html',
})
export class MovimientoFormComponent implements OnInit {
  
  idMovimiento: number | null = null;
  modoEdicion = false;

  cuentas: Cuenta[] = [];

  cargando = false;
  guardando = false;
  error = '';

  form!: FormGroup;

  constructor(
    private readonly fb: FormBuilder,
    private readonly movimientoService: MovimientoService,
    private readonly cuentaService: CuentaService,
    private readonly route: ActivatedRoute,
    private readonly router: Router
  ) {}

  ngOnInit(): void {
    this.form = this.fb.group({
        idCuenta: [null as number | null, [Validators.required]],
        tipo: ['', [Validators.required]],
        monto: [null as number | null, [Validators.required, Validators.min(0.01)]],
        descripcion: [''],
        via: ['', [Validators.required]]
    });

    this.cargarCuentas();

    const idCuentaParam = this.route.snapshot.paramMap.get('idCuenta');
    const idMovimientoParam = this.route.snapshot.paramMap.get('idMovimiento');

    if (idCuentaParam) {

      console.log('idCuentaParam', idCuentaParam);

      this.form.patchValue({
          idCuenta: Number(idCuentaParam),
      }); 
    }

    if (idMovimientoParam) {
      console.log('idMovimientoParam', idMovimientoParam);
      this.guardando = false;
      this.cargando = false;
      this.idMovimiento = Number(idMovimientoParam);
      this.modoEdicion = true;
      this.cargarMovimiento(this.idMovimiento);
    }
  }

  cargarCuentas(): void {
    this.cuentaService.getCuentas().subscribe({
      next: (data) => {
        this.cuentas = data.filter(cuenta => cuenta.activa);
      },
      error: (err) => {
        console.error(err);
        this.error = 'No se pudieron cargar las cuentas.';
      }
    });
  }

  cargarMovimiento(idMovimiento: number): void {
    this.cargando = true;
    this.error = '';

    this.movimientoService.getById(idMovimiento).subscribe({
      next: (mov) => {
        this.form.patchValue({
          idCuenta: mov.idCuenta,
          tipo: mov.tipo,
          monto: mov.monto,
          descripcion: mov.descripcion ?? '',
          via: mov.via
        });

        this.cargando = false;
      },
      error: (err) => {
        console.error(err);
        this.error = 'No se pudo cargar el movimiento.';
        this.cargando = false;
      }
    });
  }

  guardar(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.guardando = true;
    this.error = '';

    const data = {
      idCuenta: Number(this.form.value.idCuenta),
      tipo: this.form.value.tipo?.trim().toLowerCase() ?? '',
      monto: Number(this.form.value.monto),
      descripcion: this.form.value.descripcion?.trim() || null,
      via: this.form.value.via?.trim().toLowerCase() ?? ''
    };

    const peticion = this.modoEdicion && this.idMovimiento
      ? this.movimientoService.update(this.idMovimiento, data)
      : this.movimientoService.create(data);

    peticion.subscribe({
      next: () => {
        this.guardando = false;
        this.router.navigate(['/movimientos']);
      },
      error: (err) => {
        console.error(err);
        this.guardando = false;
        this.error = err.error?.mensaje ?? 'No se pudo guardar el movimiento.';
      }
    });
  }

  campoInvalido(campo: string): boolean {
    const control = this.form.get(campo);
    return !!control && control.invalid && (control.touched || control.dirty);
  }
}