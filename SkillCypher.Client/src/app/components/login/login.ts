import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router,RouterLink } from '@angular/router';
import { Auth } from '../../services/auth';

@Component({
  selector: 'app-login',
  imports: [FormsModule,CommonModule,RouterLink],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {
  email = '';
  password = '';
  errorMessage = '';

  constructor(private authService: Auth,private router: Router){}

  onLogin(): void{
    this.authService.login(this.email,this.password).subscribe({
      next:(res) => {
        this.authService.saveToken(res.token);
        this.router.navigate(['/jobs']);
      },
      error: () => {
        this.errorMessage = 'Invalid email or password.';
      }
    });
  }
}
