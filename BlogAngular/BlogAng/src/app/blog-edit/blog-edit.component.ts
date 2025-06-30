import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { BlogService } from '../services/blog.service';
import { AuthService } from '../services/auth.service';
import { FormGroup, FormBuilder, Validators, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-blog-edit',
  standalone:true,
  imports:[CommonModule,RouterLink,ReactiveFormsModule],
  templateUrl: './blog-edit.component.html',
  styleUrls: ['./blog-edit.component.css']
})
export class BlogEditComponent implements OnInit {
  blogForm: FormGroup = new FormGroup({});
  blogId: string ='';
  isLoading: boolean = false;
  errorMessage: string = '';
  blog: any;

  constructor(
    private blogService: BlogService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private fb: FormBuilder
  ) {}

  ngOnInit(): void {
    this.blogId = this.activatedRoute.snapshot.paramMap.get('id')!;
    this.fetchBlogDetails();
    this.initForm();
  }

  // Blog detaylarını API'den çekme
  fetchBlogDetails(): void {
    this.blogService.getBlogById(this.blogId).subscribe(
      (data) => {
        this.blog = data;
        this.blogForm.patchValue(this.blog);  // Formu blog verileriyle doldur
      },
      (error: HttpErrorResponse) => {
        console.error('Blog bilgileri alınırken hata oluştu', error);
      }
    );
  }

  // Blog formunu başlatma
  initForm(): void {
    this.blogForm = this.fb.group({
      baslik: ['', Validators.required],
      icerik: ['', Validators.required],
      foto: [null]
    });
  }

  // Blog güncelleme işlemi
  onUpdateBlog(): void {
    if (this.blogForm.invalid) {
      return;
    }

    this.isLoading = true;

    const formData = new FormData();
    formData.append('id', this.blogId);  
    formData.append('baslik', this.blogForm.get('baslik')!.value);
    formData.append('icerik', this.blogForm.get('icerik')!.value);
    formData.append('kategori', this.blogForm.get('kategori')!.value);
    
    const photo: File = this.blogForm.get('foto')!.value;
    if (photo) {
      formData.append('photo', photo, photo.name);
    }

    this.blogService.updateBlog(this.blogId, formData).subscribe(
      (response: any) => {
        this.isLoading = false;
        if (response.success) {
          this.router.navigate(['/blogs']);  // Güncellenmiş blog listesine yönlendirme
        }
      },
      (error: HttpErrorResponse) => {
        this.isLoading = false;
        this.errorMessage = error.error.message || 'Bir hata oluştu.';
      }
    );
  }

  // Fotoğraf seçimi
  onFileSelect(event: any): void {
    const file = event.target.files[0];
    if (file) {
      this.blogForm.patchValue({
        foto: file
      });
    }
  }

}
