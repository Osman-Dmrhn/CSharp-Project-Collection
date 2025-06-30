import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class CommentService {
  private baseUrl = 'https://localhost:7168/api/comments'; // API URL

  constructor(private http: HttpClient) {}

  addComment(blogId: string, content: string): Observable<any> {
    const token = this.getToken();
    const body = { BlogId: blogId, Content: content };
    return this.http.post(`https://localhost:7168/Comment/commentadd`, body,{
      headers:{
          'Authorization': `Bearer ${token}`
      }
  });
  }


  getToken(): string {
    return localStorage.getItem('authToken') || '';  // Token'ı localStorage'dan alabilirsiniz
  }
}
