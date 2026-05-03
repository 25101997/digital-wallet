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
  templateUrl: './transferencia-form.component.html',
})
export class TransferenciaFormComponent implements OnInit {

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
        idCuentaDestino: [null as number | null, [Validators.required]],
        monto: [null as number | null, [Validators.required, Validators.min(0.01)]],
        descripcion: [''],
    });

    this.cargarCuentas();

    const idCuentaParam = this.route.snapshot.paramMap.get('idCuenta');
  
    if (idCuentaParam) {
      this.form.patchValue({
          idCuenta: Number(idCuentaParam),
      }); 
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

  guardar(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.guardando = true;
    this.error = '';

    const data = {
      idCuenta: Number(this.form.value.idCuenta),
      idCuentaDestino: Number(this.form.value.idCuentaDestino),
      monto: Number(this.form.value.monto),
      descripcion: this.form.value.descripcion?.trim() || null,
    };

    const peticion = this.movimientoService.transferir(data);

    peticion.subscribe({
      next: () => {
        this.guardando = false;
        this.router.navigate([`/movimientos/cuenta/${data.idCuenta}`]);
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