import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AdminBlogService {
  private readonly apiUrl = 'https://localhost:7168/api/admin/blogs'; // API URL'i

  constructor(private http: HttpClient) {}

  getAllBlogs(): Observable<any> {
    const token = this.getToken();
    return this.http.get(`${this.apiUrl}/getBlogs`,{headers:{
        'Authorization': `Bearer ${token}`
    }});
  }

  approveBlog(id: string): Observable<any> {
    const token = this.getToken();
    return this.http.post(`${this.apiUrl}/approve/${id}`,{id},{headers:{
        'Authorization': `Bearer ${token}`
    }});
  }

  deleteBlog(id: string): Observable<any> {
    const token = this.getToken();
    return this.http.post(`${this.apiUrl}/delete/${id}`, { id },{headers:{
        'Authorization': `Bearer ${token}`
    }});
  }

  getBlogDetails(id: string): Observable<any> {
    const token = this.getToken();
    return this.http.get(`${this.apiUrl}/${id}`,{headers:{
        'Authorization': `Bearer ${token}`
    }});
  }

  getToken(): string {
    return localStorage.getItem('admin_token') || '';
  }
}
