import { Component, OnInit } from '@angular/core';
import { AdminService } from '../services/admin.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-admin-settings',
  standalone:true,
  imports:[CommonModule,FormsModule],
  templateUrl: './admin-setttings.component.html',
  styleUrls: ['./admin-setttings.component.css'],
})
export class AdminSettingsComponent implements OnInit {
  managers: any[] = [];
  selectedManager: any = null;
  newManager = { kullaniciAdi: '', email: '', sifre: '', rol: 'Admin' };

  constructor(private adminService: AdminService) {}

  ngOnInit(): void {
    this.loadManagers();
  }

  loadManagers(): void {
    this.adminService.getManagers().subscribe({
      next: (data) => {
        this.managers = data;
      },
      error: (err) => {
        console.error('Yöneticileri yüklerken hata oluştu!', err);
      },
    });
  }

  addManager(): void {
    this.adminService.addManager(this.newManager).subscribe({
      next: (response) => {
        alert(response.message);
        this.loadManagers();
        this.newManager = { kullaniciAdi: '', email: '', sifre: '', rol: 'Admin' };
      },
      error: (err) => {
        console.error('Yönetici eklerken hata oluştu!', err);
      },
    });
  }

  editManager(manager: any): void {
    this.selectedManager = { ...manager }; // Düzenlenecek yöneticiyi seç
  }

  saveManager(): void {
    if (this.selectedManager) {
      this.adminService.editManager(this.selectedManager.id, this.selectedManager).subscribe({
        next: (response) => {
          alert(response.message);
          this.loadManagers();
          this.selectedManager = null;
        },
        error: (err) => {
          console.error('Yönetici güncellerken hata oluştu!', err);
        },
      });
    }
  }

  deleteManager(id: string): void {
    if (confirm('Bu yöneticiyi silmek istediğinize emin misiniz?')) {
      this.adminService.deleteManager(id).subscribe({
        next: (response) => {
          alert(response.message);
          this.loadManagers();
        },
        error: (err) => {
          console.error('Yönetici silerken hata oluştu!', err);
        },
      });
    }
  }
}
