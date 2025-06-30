import { Component, OnInit } from '@angular/core';
import { UserService } from '../services/user.service';
import { ChangePasswordModel } from '../models/ChangePasswordModel';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-user-pass',
  standalone:true,
  imports:[CommonModule,FormsModule],
  templateUrl: './user-pass.component.html',
  styleUrls: ['./user-pass.component.css']
})
export class UserPassComponent implements OnInit {
  model: ChangePasswordModel = new ChangePasswordModel();
  errorMessage: string = '';
  successMessage: string = '';
  isLoading: boolean = false;

  constructor(private userService: UserService, private router: Router) {}

  ngOnInit(): void {}

  changePassword(): void {
    if (this.model.NewPassword !== this.model.ConfirmPassword) {
      this.errorMessage = "Yeni şifre ve tekrar birbirine uymuyor.";
      return;
    }

    this.isLoading = true;
    this.userService.changePassword(this.model).subscribe(
      (response) => {
        this.successMessage = response.message;
        this.errorMessage = '';
        this.isLoading = false;
      },
      (error) => {
        this.errorMessage = error.error.message || 'Şifre değiştirilemedi.';
        this.successMessage = '';
        this.isLoading = false;
      }
    );
  }
}
