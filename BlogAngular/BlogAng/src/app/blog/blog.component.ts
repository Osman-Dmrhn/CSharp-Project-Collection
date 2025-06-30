import { Component, OnInit } from '@angular/core';
import { BlogService } from '../services/blog.service';
import { Category } from '../models/category';
import { BlogAddModel } from '../models/BlogAddModel';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-blog',
  templateUrl: './blog.component.html',
  styleUrls: ['./blog.component.css']
})
export class BlogComponent implements OnInit {

  categories: Category[] = [];
  blogAddModel: BlogAddModel = new BlogAddModel();
  selectedPhoto: File | null = null;
  selectedCategory: string = '';  // Kategori seçimi için

  constructor(private blogService: BlogService, private http: HttpClient) { }

  ngOnInit(): void {
    this.loadCategories();
  }

  loadCategories() {
    this.blogService.getCategories().subscribe((categories: Category[]) => {
      this.categories = categories;
    });
  }

  onPhotoSelect(event: any) {
    this.selectedPhoto = event.target.files[0];
  }

  addBlog() {
    if (this.selectedPhoto && this.selectedCategory && this.blogAddModel.Baslik && this.blogAddModel.Icerik) {
      const formData = new FormData();
      
      // Blog bilgilerini formData'ya ekliyoruz
      formData.append('Baslik', this.blogAddModel.Baslik);
      formData.append('Icerik', this.blogAddModel.Icerik);
      formData.append('KategoriAdi', this.selectedCategory);

      // Fotoğrafı formData'ya ekliyoruz
      formData.append('photo', this.selectedPhoto);

      // API'ye gönderiyoruz
      this.blogService.addBlog(formData).subscribe(response => {
        console.log(response);
        alert('Blog successfully added!');
      }, error => {
        console.error(error);
        alert('An error occurred while adding the blog.');
      });
    } else {
      alert('Please fill all the fields and select a photo.');
    }
  }
}
