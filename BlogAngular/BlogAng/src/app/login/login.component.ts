import { Component } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  standalone:true,
  imports:[FormsModule,CommonModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  logindata={
    email:  '',
    Password: ''
  };

  errorMessage: string = '';

  constructor(private authService: AuthService, private router: Router) {}

  onLogin(): void {
    this.authService.login(this.logindata).subscribe({
      next: (response) => {
        localStorage.setItem('token', response.token);
        this.router.navigate(['/home']);
      },
      error: (err) => {
        this.errorMessage = err.error.message || 'Login failed.';
      }
    });
  }
}