import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  ValidationErrors,
  ValidatorFn,
  Validators
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

/**
 * Password must have upper, lower, and a digit.
 * Matches the backend SignUpRequestValidator rules.
 */
function passwordComplexity(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const value = control.value as string;
    if (!value) return null;

    const errors: ValidationErrors = {};
    if (!/[A-Z]/.test(value)) errors['noUpper'] = true;
    if (!/[a-z]/.test(value)) errors['noLower'] = true;
    if (!/[0-9]/.test(value)) errors['noDigit'] = true;

    return Object.keys(errors).length ? errors : null;
  };
}

@Component({
  selector: 'app-signup',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterLink,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatCardModule
  ],
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.scss']
})
export class SignUpComponent {
  private readonly fb = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  public readonly form: FormGroup = this.fb.group({
    firstName: ['', [Validators.required, Validators.maxLength(100)]],
    lastName: ['', [Validators.required, Validators.maxLength(100)]],
    email: ['', [Validators.required, Validators.email, Validators.maxLength(256)]],
    password: ['', [
      Validators.required,
      Validators.minLength(8),
      Validators.maxLength(128),
      passwordComplexity()
    ]]
  });

  public submitting = false;
  public serverError: string | null = null;

  public onSubmit(): void {
    this.serverError = null;

    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.submitting = true;

    this.authService.signUp(this.form.value).subscribe({
      next: () => this.router.navigate(['/spacex']),
      error: (err: HttpErrorResponse) => {
        this.submitting = false;
        this.serverError = this.extractErrorMessage(err);
      }
    });
  }

  private extractErrorMessage(err: HttpErrorResponse): string {
    if (err.status === 409) {
      return err.error?.detail ?? 'An account with that email already exists.';
    }
    if (err.status === 400 && err.error?.errors) {
      const messages = Object.values(err.error.errors as Record<string, string[]>).flat();
      return messages.join(' ');
    }
    return 'Something went wrong. Please try again.';
  }
}
