import { Component, OnInit } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { AdminService } from '../services/admin.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-admin-home',
  standalone:true,
  imports:[CommonModule,RouterLink],
  templateUrl: './admin-home.component.html',
  styleUrls: ['./admin-home.component.css']
})
export class AdminHomeComponent implements OnInit {
  adminEmail: string = '';
  isLoading: boolean = true;
  errorMessage: string = '';

  constructor(private adminService: AdminService, private router: Router) {}

  ngOnInit(): void {
  }


  // Çıkış işlemi
  logout(): void {
    this.adminService.logout().subscribe(
      () => {
        localStorage.removeItem('admin_token');
        this.router.navigate(['/admin/login']);
      },
      (error) => {
        this.errorMessage = 'Çıkış yapılamadı, tekrar deneyin.';
      }
    );
  }
}
