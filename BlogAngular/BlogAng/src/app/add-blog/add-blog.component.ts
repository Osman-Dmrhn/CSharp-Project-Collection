import { Component, OnInit } from '@angular/core';
import { BlogService } from '../services/blog.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-add-blog',
  standalone:true,
  imports:[CommonModule,FormsModule],
  templateUrl: './add-blog.component.html',
  styleUrls: ['./add-blog.component.css']
})
export class AddBlogComponent implements OnInit {
  blogData = {
    Baslik: '',
    Icerik: '',
    KategoriAdi: ''
  };
  photo: File | null = null;
  errorMessage: string = '';
  categories: { id: string, kategoriAdi: string }[] = [];

  constructor(private blogService: BlogService, private router: Router) {}

  ngOnInit(): void {
    this.loadCategories(); 
  }

  loadCategories(): void {
    this.blogService.getCategories().subscribe({
      next: (data: any) => {
        
        console.log('Gelen veri:', data);
  
        
        if (data && data.$values && Array.isArray(data.$values)) {
          this.categories = data.$values; 
          console.log('Kategoriler:', this.categories);
        } else {
          console.log('Kategoriler boş geldi veya undefined');
        }
      },
      error: (err) => {
        console.log('Hata:', err);
        this.errorMessage = 'Error: ' + err.message;
      }
    });
  }

  onFileChange(event: any): void {
    if (event.target.files.length > 0) {
      this.photo = event.target.files[0];
    }
  }

  onSubmit(): void {
    const formData = new FormData();
    formData.append('Baslik', this.blogData.Baslik);
    formData.append('Icerik', this.blogData.Icerik);
    formData.append('KategoriAdi', this.blogData.KategoriAdi);

    if (this.photo) {
      formData.append('photo', this.photo, this.photo.name);
    }

    this.blogService.addBlog(formData).subscribe({
      next: () => {
        this.router.navigate(['/blog-list']); // Redirect to blog list or another page
      },
      error: (err) => {
        this.errorMessage = 'Error: ' + err.message;
      }
    });
  }
}
