import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  ReactiveFormsModule,
  Validators,
  FormGroup
} from '@angular/forms';
import { Router } from '@angular/router';

import { AuthService } from '../../../../core/services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit {
  cargando = false;
  error = '';

  form!: FormGroup;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {}


  ngOnInit(): void {
    this.form = this.fb.group({
        usernameOrEmail: ['', [Validators.required]],
        password: ['', [Validators.required]]
    });
  }

  login(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.cargando = true;
    this.error = '';

    const request = {
      usernameOrEmail: this.form.value.usernameOrEmail ?? '',
      password: this.form.value.password ?? ''
    };

    this.authService.login(request).subscribe({
      next: () => {
        this.cargando = false;
        this.router.navigate(['/cuentas']);
      },
      error: (err) => {
        this.cargando = false;
        this.error = err.error?.error ?? 'No se pudo iniciar sesión';
      }
    });
  }
}