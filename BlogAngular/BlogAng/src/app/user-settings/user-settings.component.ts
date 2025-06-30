import { Component, OnInit } from '@angular/core';
import { UserService } from '../services/user.service';
import { User } from '../models/user';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-user-settings',
  standalone:true,
  imports:[CommonModule,FormsModule,RouterLink],
  templateUrl: './user-settings.component.html',
  styleUrls: ['./user-settings.component.css']
})
export class UserSettingsComponent implements OnInit {
  user: User | null = null;
  editUser: User = new User();
  errorMessage: string = '';
  successMessage: string = '';
  isLoading: boolean = true;

  constructor(private userService: UserService, private router: Router) {}

  ngOnInit(): void {
    this.loadUser();
  }

  loadUser(): void {
    this.userService.getUser().subscribe(
      (data: User) => {
        this.user = data;
        this.editUser = { ...data };  // Initialize edit form with user data
        this.isLoading = false;  // Yükleme tamamlandığında
      },
      (error) => {
        this.errorMessage = error.error.message || 'Kullanıcı bilgileri alınamadı.';
        this.isLoading = false;  // Yükleme tamamlandığında
      }
    );
  }

  saveUserChanges(): void {
    this.userService.editUser(this.editUser).subscribe(
      (response) => {
        this.successMessage = response.message;
        this.errorMessage = '';
      },
      (error) => {
        this.errorMessage = error.error.message || 'Profil güncellenemedi.';
        this.successMessage = '';
      }
    );
  }
}
