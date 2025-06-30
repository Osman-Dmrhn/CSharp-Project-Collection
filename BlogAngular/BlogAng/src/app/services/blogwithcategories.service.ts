import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class BlogWithCategoriesService {
  private apiUrl = 'https://localhost:7168/api/BlogWithCategory/getBlogsWithCategories';  // API URL'sini kendi backend'ine göre değiştir.
  constructor(private http: HttpClient) { }

  getBlogsWithCategories(): Observable<{ Blogs: any[], Kategoriler: any[] }> {
    return this.http.get<{ Blogs: any[], Kategoriler: any[] }>(this.apiUrl);
  }

  // Blog detayını ID'ye göre çeker
  getBlogById(blogId: string): Observable<any> {
    return this.http.get<any>(`https://localhost:7168/api/Blog/GetBlog/${blogId}`);
  }

  // Fotoğrafı çekmek için metod
  getPhoto(fileName: string): Observable<Blob> {
    return this.http.get(`https://localhost:7168/api/Blog/getPhoto/${fileName}`, { responseType: 'blob' });
  }
}
