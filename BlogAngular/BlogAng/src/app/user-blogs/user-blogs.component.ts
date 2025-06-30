import { Component, OnInit } from '@angular/core';
import { BlogService } from '../services/blog.service';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { BlogWithCategoriesService } from '../services/blogwithcategories.service';
import { SafeUrl,DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-user-blogs',
  standalone:true,
  imports:[CommonModule,RouterLink,FormsModule],
  templateUrl: './user-blogs.component.html',
  styleUrls: ['./user-blogs.component.css']
})
export class UserBlogsComponent implements OnInit {
  blogs: any[] = [];
  isLoading = true;
  errorMessage: string | null = null;
  imageUrls: { [key: string]: SafeUrl } = {};

  constructor(private blogService: BlogService,private sanitizer: DomSanitizer,private blogwithcategoryService:BlogWithCategoriesService) {}

  ngOnInit(): void {
    this.UserBlogGet();
  }

  UserBlogGet(){
    this.blogService.getUserBlogs().subscribe({
      next: (data) => {
        if (!data?.$values || data.$values.length === 0) {
          this.errorMessage = 'Henüz blog bulunmamaktadır.';
          this.blogs = []; // Boş bir liste atanır
        }else
        this.blogs = data.$values||[];
        this.isLoading = false;

         // Fotoğrafları yükle
         this.blogs.forEach((blog) => {
          if (blog.resimPath) {
            this.blogwithcategoryService.getPhoto(blog.resimPath).subscribe((blob) => {
              const objectURL = URL.createObjectURL(blob);
              this.imageUrls[blog.id] = this.sanitizer.bypassSecurityTrustUrl(objectURL);
            });
          }
        });
      },
      error: (error) => {
        this.errorMessage = 'Bloglar yüklenirken bir hata oluştu.';
        this.isLoading = false;
      }
    });
  }

  deleteBlog(id: string): void {
    this.blogService.deleteBlog(id).subscribe(
      (response) => {
        this.errorMessage = response.message;
        this.UserBlogGet(); // Blog listesini güncelle
      },
      (error) => {
        this.errorMessage = error.error?.message || 'Blog silme başarısız oldu.';
      }
    );
  }
}
