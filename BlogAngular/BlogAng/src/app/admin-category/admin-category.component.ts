import { Component, OnInit } from '@angular/core';
import { CategoryService } from '../services/admin-category.service';
import { Category } from '../models/category';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-admin-category',
  standalone:true,
  imports:[CommonModule,FormsModule],
  templateUrl: './admin-category.component.html',
  styleUrls: ['./admin-category.component.css']
})
export class AdminCategoryComponent implements OnInit {
  categories: any[] = [];
  newCategory: string = '';
  selectedCategory: any = null;
  message: string = '';

  constructor(private categoryService: CategoryService) { }

  ngOnInit(): void {
    this.getCategories();
  }

  getCategories(): void {
    this.categoryService.getCategories().subscribe(
      (data: any) => {
        
        this.categories = data.$values.map((category: any) => ({
          id: category.id,
          kategoriAdi: category.kategoriAdi,
          bloglar: category.bloglar.$values 
        }));
      },
      (error) => {
        this.message = 'Kategoriler alınırken hata oluştu!';
      }
    );
  }

  addCategory(): void {
    if (this.newCategory.trim() === '') {
      this.message = 'Kategori adı boş olamaz!';
      return;
    }

    const categoryDto: Category = { KategoriAdi: this.newCategory };
    this.categoryService.addCategory(categoryDto).subscribe(
      (response) => {
        this.message = response.message;
        this.getCategories(); 
        this.newCategory = ''; 
      },
      (error) => {
        this.message = 'Kategori eklenirken hata oluştu!';
      }
    );
  }

  editCategory(): void {
    if (this.selectedCategory && this.selectedCategory.KategoriAdi.trim() !== '') {
      const categoryDto: Category = { KategoriAdi: this.selectedCategory.KategoriAdi };
      this.categoryService.editCategory(this.selectedCategory.id, categoryDto).subscribe(
        (response) => {
          this.message = response.message;
          this.getCategories(); 
          this.selectedCategory = null; 
        },
        (error) => {
          this.message = 'Kategori güncellenirken hata oluştu!';
        }
      );
    }
  }

  deleteCategory(id: string): void {
    this.categoryService.deleteCategory(id).subscribe(
      (response) => {
        this.message = response.message;
        this.getCategories(); // Listeyi güncelle
        this.message = 'Kategori Başarıyla Silindi';
      },
      (error) => {
        this.message = 'Kategori silinirken hata oluştu!';
      }
    );
  }

  selectCategory(category: any): void {
    this.selectedCategory = { ...category };
  }
}
