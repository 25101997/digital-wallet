import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';

import { CuentaService } from '../../services/cuenta.service';

@Component({
  selector: 'app-cuenta-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './cuenta-form.component.html'
})
export class CuentaFormComponent implements OnInit {
    
  idCuenta: number | null = null;
  modoEdicion = false;

  cargando = false;
  guardando = false;
  error = '';
  form!: FormGroup;

  constructor(
    private readonly fb: FormBuilder,
    private readonly cuentaService: CuentaService,
    private readonly route: ActivatedRoute,
    private readonly router: Router
  ){}

  ngOnInit(): void {

    this.form = this.fb.group({
      nombre: ['', [Validators.required, Validators.maxLength(100)]],
      tipo: ['', [Validators.required, Validators.maxLength(50)]],
      activa: [true, [Validators.required]]
    });

    const id = this.route.snapshot.paramMap.get('idCuenta');

    if (id) {
      this.idCuenta = Number(id);
      this.modoEdicion = true;
      this.cargarCuenta(this.idCuenta);
    }
  }

  cargarCuenta(idCuenta: number): void {
    this.cargando = true;
    this.error = '';

    this.cuentaService.getCuentaById(idCuenta).subscribe({
      next: (cuenta) => {
        this.form.patchValue({
          nombre: cuenta.nombre,
          tipo: cuenta.tipo,
          activa: cuenta.activa
        });

        this.cargando = false;
      },
      error: (err) => {
        console.error(err);
        this.error = 'No se pudo cargar la cuenta.';
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
      nombre: this.form.value.nombre?.trim() ?? '',
      tipo: this.form.value.tipo?.trim() ?? '',
      activa: this.form.value.activa ?? true
    };

    const peticion = this.modoEdicion && this.idCuenta
      ? this.cuentaService.updateCuenta(this.idCuenta, data)
      : this.cuentaService.createCuenta(data);

    peticion.subscribe({
      next: () => {
        this.guardando = false;
        this.router.navigate(['/cuentas']);
      },
      error: (err) => {
        console.error(err);
        this.guardando = false;
        this.error = err.error?.mensaje ?? 'No se pudo guardar la cuenta.';
      }
    });
  }

  campoInvalido(campo: string): boolean {
    const control = this.form.get(campo);
    return !!control && control.invalid && (control.touched || control.dirty);
  }
}