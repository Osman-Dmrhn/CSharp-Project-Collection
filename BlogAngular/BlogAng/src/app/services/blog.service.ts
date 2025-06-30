import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../enviroment/enviroment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class BlogService {

  private apiUrl = 'https://localhost:7168/api/Blog';

  constructor(private http: HttpClient) {}

  addBlog(blogData: FormData): Observable<any> {
    const token = this.getToken();
  
    return this.http.post(`${this.apiUrl}/AddBlog`, blogData, {
      headers: {
        'Authorization': `Bearer ${token}`
      },
      withCredentials: true  
    });
  }

  getUserBlogs(): Observable<any> {
    const token = this.getToken();
    return this.http.get<any>(`${this.apiUrl}/GetBlogById`,{
      headers:{
          'Authorization': `Bearer ${token}`
      }
    });
  }

  deleteBlog(id: string): Observable<any> {
    const token = this.getToken();
    return this.http.post(`${this.apiUrl}/delete/${id}`, { id },{headers:{
        'Authorization': `Bearer ${token}`
    }});
  }

  updateBlog(id: string, formData: FormData): Observable<any> {
    const token = this.getToken();
    return this.http.post<any>(`${this.apiUrl}/update/${id}`, formData,{headers:{
      'Authorization': `Bearer ${token}`
  }});
  }

  getBlogById(blogId: string): Observable<any> {
    return this.http.get<any>(`https://localhost:7168/api/Blog/GetBlog/${blogId}`);
  }
  
  getToken(): string {
    return localStorage.getItem('authToken') || '';  // Token'ı localStorage'dan alabilirsiniz
  }

  getCategories(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/GetCategories`);
  }

   
   getBlogsWithCategories(): Observable<any> {
    return this.http.get<any>(this.apiUrl);  
  }
}
