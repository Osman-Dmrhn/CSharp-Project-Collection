import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { BlogWithCategoriesService } from '../services/blogwithcategories.service'; // Blog servisi
import { CommentService } from '../services/comment.service'; // Yorum servisi
import { CommonModule } from '@angular/common';
import { AuthService } from '../services/auth.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-blog-detail',
  standalone: true,
  imports: [RouterLink, CommonModule,FormsModule],
  templateUrl: './blog-detail.component.html',
  styleUrls: ['./blog-detail.component.css'],
})
export class BlogDetailComponent implements OnInit {
  blog: any = {}; // Blog verisi
  blogId: string = ''; // Blog ID'si
  imageUrl: string = ''; // Fotoğraf URL'si
  comments: any[] = []; // Yorumlar
  newComment: string = ''; // Yeni yorum

  constructor(
    private route: ActivatedRoute,
    private blogwithcategoryService: BlogWithCategoriesService, 
    private commentService: CommentService, 
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    // Route parametresini al
    this.blogId = this.route.snapshot.paramMap.get('id')!;
    this.fetchBlogDetails(this.blogId);
  }

  // Blog detaylarını çekmek için
  fetchBlogDetails(blogId: string): void {
    this.blogwithcategoryService.getBlogById(blogId).subscribe({
      next: (data: any) => {
        this.blog = data;
        this.fetchPhoto(this.blog.resimPath); // Fotoğrafı çek
        this.comments=data.yorumlar.$values||[];
      },
      error: (err) => {
        console.error('Blog detayları çekilirken hata oluştu!', err);
      },
    });
  }

  // Fotoğrafı çekmek için
  fetchPhoto(fileName: string): void {
    this.blogwithcategoryService.getPhoto(fileName).subscribe({
      next: (data: Blob) => {
        const url = URL.createObjectURL(data);
        this.imageUrl = url; // Fotoğraf URL'sini sakla
      },
      error: (err) => {
        console.error('Fotoğrafı çekerken hata oluştu!', err);
      },
    });
  }

  

  // Yeni yorum eklemek için
  addComment(): void {
    if (this.newComment.trim() === '') {
      alert('Yorum boş olamaz!');
      return;
    }

    // Kullanıcı giriş yapmamışsa login sayfasına yönlendir
    if (!this.authService.checkAuthenticationStatus()) {
    alert('Yorum eklemek için giriş yapmalısınız!');
    this.router.navigate(['/login']); // Giriş yapma sayfasına yönlendir
    return;
    }

    this.commentService.addComment(this.blogId, this.newComment).subscribe({
      next: (response) => {
        alert(response.message);
        this.newComment = '';
      },
      error: (err) => {
        console.error('Yorum eklenirken hata oluştu!', err);
      },
    });
  }

  // Geri gitmek için
  navigateBack(): void {
    this.router.navigate(['/']);
  }
}
