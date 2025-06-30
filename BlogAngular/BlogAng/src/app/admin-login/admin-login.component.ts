import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AdminService } from '../services/admin.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-admin-login',
  standalone:true,
  imports:[CommonModule,FormsModule],
  templateUrl: './admin-login.component.html',
  styleUrls: ['./admin-login.component.css'],
})
export class AdminLoginComponent {
  email: string = '';
  password: string = '';
  errorMessage: string = '';
  isLoading: boolean = false;

  constructor(private adminService: AdminService, private router: Router) {}

  login(): void {
    this.isLoading = true;
    this.adminService.login(this.email, this.password).subscribe(
      (response: any) => {
        if (response.success) {
          localStorage.setItem('admin_token', response.token); // Admin için ayrı token saklıyoruz
          this.router.navigate(['/addhome']);
        } else {
          this.errorMessage = response.message;
        }
        this.isLoading = false;
      },
      (error) => {
        this.errorMessage = error.error.message || 'Giriş başarısız!';
        this.isLoading = false;
      }
    );
  }
}
