import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable,map } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AdminService {
  private apiUrl = 'https://localhost:7168/api/admin/auth'; // Admin API'nin temel yolu

  constructor(private http: HttpClient) {}

  // Admin giriş işlemi
  login(email: string, password: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/login`, { email, password });
  }

  // Admin çıkış işlemi
  logout(): Observable<any> {
    return this.http.post(`${this.apiUrl}/logout`, {});
  }

  // Admin yetkilendirme durumu kontrolü
  checkAuth(): Observable<any> {
    return this.http.get(`${this.apiUrl}/check-auth`);
  }

  getManagers(): Observable<any[]> {
    const token = this.getToken();
    return this.http.get<any>(`https://localhost:7168/api/admin/managers/GetManagers`,{headers:{
      'Authorization': `Bearer ${token}`
  }}).pipe(
      map((response) => response.$values) // Gelen veri içindeki `$values`'u alıyoruz
    );
  }

  addManager(admin: any): Observable<any> {
    const token = this.getToken();
    return this.http.post<any>(`https://localhost:7168/api/admin/managers/AddManager`, admin,{headers:{
      'Authorization': `Bearer ${token}`
  }});
  }

  getManagerById(id: string): Observable<any> {
    const token = this.getToken();
    return this.http.get<any>(`https://localhost:7168/api/admin/managers/GetManagerById/${id}`,{headers:{
      'Authorization': `Bearer ${token}`
  }});
  }

  editManager(id: string, admin: any): Observable<any> {
    const token = this.getToken();
    return this.http.put<any>(`https://localhost:7168/api/admin/managers/EditManager/${id}`, admin,{headers:{
      'Authorization': `Bearer ${token}`
  }});
  }

  deleteManager(id: string): Observable<any> {
    const token = this.getToken();
    return this.http.delete<any>(`https://localhost:7168/api/admin/managers/DeleteManager/${id}`,{headers:{
      'Authorization': `Bearer ${token}`
  }});
  }

  getToken(): string {
    return localStorage.getItem('admin_token') || '';
  }
}
