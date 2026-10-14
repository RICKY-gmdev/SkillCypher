import { CommonModule } from '@angular/common';
import { Component, ElementRef, ViewChild} from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router,RouterLink } from '@angular/router';
import { Auth } from '../../services/auth';

@Component({
  selector: 'app-login',
  imports: [FormsModule,CommonModule,RouterLink],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login{
  email = '';
  password = '';
  errorMessage = '';

  authState: 'idle' | 'loading' | 'success' | 'error' = 'idle';



  constructor(private authService: Auth,private router: Router){}

  onLogin(): void{

    this.authState = 'loading';
    this.errorMessage = '';

    this.authService.login(this.email,this.password).subscribe({
      next:(res) => {
        this.authService.saveToken(res.token);
        this.authService.SaveRole(res.role);
        this.authState = 'success';

        if(res.role === "Recruiter")
        {
          this.router.navigate(['/recruiter-dashboard']);
        }else{
          this.router.navigate(['/dashboard']);
        }
      },
      error: () => {
        this.authState = 'error';
        this.errorMessage = 'Invalid email or password.';
      }
    });
  }

}
