import { Component, OnInit } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { BlogWithCategoriesService } from '../services/blogwithcategories.service';
import { CommonModule } from '@angular/common';
import { SafeUrl, DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent implements OnInit {
  blogs: any[] = [];
  filteredBlogs: any[] = [];
  categories: string[] = [];
  isAuthenticated: boolean = false;
  imageUrls: { [key: string]: SafeUrl } = {};
  selectedCategory: string | null = null;

  constructor(
    private blogwithcategoryService: BlogWithCategoriesService,
    private router: Router,
    private authService: AuthService,
    private sanitizer: DomSanitizer
  ) {}

  ngOnInit(): void {
    this.fetchBlogsWithCategories();
    this.authService.isAuthenticated.subscribe((isAuthenticated) => {
      this.isAuthenticated = isAuthenticated;
    });
  }

  fetchBlogsWithCategories(): void {
    this.blogwithcategoryService.getBlogsWithCategories().subscribe({
      next: (data: any) => {
        if (!data?.$values || data.$values.length === 0) {
          console.error('Henüz blog bulunmamaktadır.');
          this.blogs = []; // Boş bir liste atanır
        }else
        this.blogs = data.$values || [];
        this.filteredBlogs = this.blogs;
        this.categories = this.extractCategories(this.blogs);

        this.blogs.forEach((blog) => {
          if (blog.resimPath) {
            this.blogwithcategoryService.getPhoto(blog.resimPath).subscribe((blob) => {
              const objectURL = URL.createObjectURL(blob);
              this.imageUrls[blog.id] = this.sanitizer.bypassSecurityTrustUrl(objectURL);
            });
          }
        });
      },
      error: (err) => {
        console.error('Blogları çekerken bir hata oluştu!', err);
      },
    });
  }

  extractCategories(blogs: any[]): string[] {
    const categoriesSet = new Set<string>();
    blogs.forEach((blog) => {
      blog.kategoriler?.$values?.forEach((kategori: any) => {
        categoriesSet.add(kategori.kategoriAdi);
      });
    });
    return Array.from(categoriesSet);
  }

  filterByCategory(category: string): void {
    this.selectedCategory = category;
    this.filteredBlogs = this.blogs.filter((blog) =>
      blog.kategoriler.$values.some((kategori: any) => kategori.kategoriAdi === category)
    );
  }

  clearFilter(): void {
    this.selectedCategory = null;
    this.filteredBlogs = this.blogs;
  }

  navigateToBlogDetail(blogId: string): void {
    this.router.navigate(['/blog', blogId]);
  }

  onLogout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
