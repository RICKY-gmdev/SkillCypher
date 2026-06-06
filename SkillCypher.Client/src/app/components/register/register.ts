import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { Auth } from '../../services/auth';

@Component({
  selector: 'app-register',
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './register.html',
  styleUrl: './register.css',
})
export class Register {
  name = '';
  email = '';
  password = '';
  confirmPassword = '';
  role: 'Applicant' | 'Recruiter' = 'Applicant'
  showPassword = false;
  errorMessage = '';
  loading = false;

  constructor(private auth: Auth, private router: Router) {}

  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
  }

  get passwordMismatch(): boolean {
    return this.password !== '' && this.confirmPassword !== '' && this.password !== this.confirmPassword;
  }

  onRegister(): void {
    this.errorMessage = '';

    if (!this.email) {
      this.errorMessage = 'Please provide an email address.';
      return;
    }

    if (this.password.length < 6) {
      this.errorMessage = 'Password must be at least 6 characters.';
      return;
    }

    if (this.passwordMismatch) {
      this.errorMessage = 'Passwords do not match.';
      return;
    }

    this.loading =true;
    this.auth.register(this.name,this.email,this.password,this.role).subscribe({
      next:() => {
        this.loading = false;
        this.router.navigate(['/login']);
      },
      error: (err) => {
        this.loading = false;
        this.errorMessage = err?.error?.error || err?.message || 'Registration failed'
      },
      
    });
  }
}
