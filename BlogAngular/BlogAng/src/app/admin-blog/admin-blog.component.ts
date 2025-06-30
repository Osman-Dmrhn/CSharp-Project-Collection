import { Component, OnInit } from '@angular/core';
import { AdminBlogService } from '../services/admin-blog.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { error } from 'console';

@Component({
  selector: 'app-admin-blog',
  standalone:true,
  imports:[CommonModule,FormsModule],
  templateUrl: './admin-blog.component.html',
  styleUrls: ['./admin-blog.component.css']
})
export class AdminBlogComponent implements OnInit {
  blogs: any[] = [];
  selectedBlog: any = null;
  message: string = '';

  constructor(private blogService: AdminBlogService) {}

  ngOnInit(): void {
    this.getAllBlogs();
  }

  getAllBlogs(): void {
    this.blogService.getAllBlogs().subscribe({
      next: (data: any) => {
        this.blogs = data.$values || [];
      },
      error: (err) => {
        console.error('Blogları çekerken bir hata oluştu!', err);
      }
    }
    );
  }

  approveBlog(id: string): void {
    this.blogService.approveBlog(id).subscribe(
      (response) => {
        this.message = response.message;
        this.getAllBlogs(); // Blog listesini güncelle
      },
      (error) => {
        this.message = error.error?.message || 'Blog onaylama başarısız oldu.';
      }
    );
  }

  deleteBlog(id: string): void {
    this.blogService.deleteBlog(id).subscribe(
      (response) => {
        this.message = response.message;
        this.getAllBlogs(); // Blog listesini güncelle
      },
      (error) => {
        this.message = error.error?.message || 'Blog silme başarısız oldu.';
      }
    );
  }

  getBlogDetails(id: string): void {
    this.blogService.getBlogDetails(id).subscribe(
      (response) => {
        this.selectedBlog = response.data;
      },
      (error) => {
        this.message = error.error?.message || 'Blog ayrıntıları getirilemedi.';
      }
    );
  }
}
