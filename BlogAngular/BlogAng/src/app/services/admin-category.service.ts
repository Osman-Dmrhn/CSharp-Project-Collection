import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Category } from '../models/category';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  private apiUrl = 'https://localhost:7168/api/admin/categories'; // API url'ini doğru girin

  constructor(private http: HttpClient) { }

  getCategories(): Observable<any[]> {
    const token = this.getToken();
    return this.http.get<any[]>(`${this.apiUrl}`,{headers:{
      'Authorization': `Bearer ${token}`
  }});
  }

  addCategory(categoryDto: Category): Observable<any> {
    const token = this.getToken();
    return this.http.post<any>(`${this.apiUrl}`, categoryDto,{headers:{
      'Authorization': `Bearer ${token}`
  }});
  }

  editCategory(id: string, categoryDto: Category): Observable<any> {
    const token = this.getToken();
    return this.http.put<any>(`${this.apiUrl}/${id}`, categoryDto,{headers:{
      'Authorization': `Bearer ${token}`
  }});
  }

  deleteCategory(id: string): Observable<any> {
    const token = this.getToken();
    return this.http.delete<any>(`${this.apiUrl}/${id}`,{headers:{
      'Authorization': `Bearer ${token}`
  }});
  }

  getToken(): string {
    return localStorage.getItem('admin_token') || '';  // Token'ı localStorage'dan alabilirsiniz
  }
}
