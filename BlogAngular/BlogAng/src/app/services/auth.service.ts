import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { Router } from '@angular/router';
import { HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private isAuthenticatedSubject: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(this.checkAuthenticationStatus());
  private apiUrl = 'https://localhost:7168/api/Login';

  constructor(private http: HttpClient, private router: Router) {}

  checkAuthenticationStatus(): boolean {
    if (typeof window !== 'undefined') {  // Tarayıcıda mı çalışıyoruz?
      return !!localStorage.getItem('authToken');  // Token varsa true döner
    }
    return false;
  }

  get isAuthenticated(): Observable<boolean> {
    return this.isAuthenticatedSubject.asObservable();
  }

  // Login işlemi
  login(logindata: any): Observable<any> {
    return this.http.post<{ token: string }>(`${this.apiUrl}/login`, logindata).pipe(
      tap(response => {
        if (typeof window !== 'undefined') {  // Tarayıcıda mı çalışıyoruz?
          // Login başarılı olduğunda token'ı localStorage'a kaydet
          localStorage.setItem('authToken', response.token);
        }
        // Kullanıcının oturum açtığını güncelle
        this.isAuthenticatedSubject.next(true);
      })
    );
  }

  register(userData: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/register`, userData, { withCredentials: true }).pipe(
      tap(() => {
        this.isAuthenticatedSubject.next(true);  // Giriş başarılı olduğunda
      })
    );
  }

// Logout işlemi
logout(): Observable<any> {
  if (typeof window !== 'undefined') {  // Tarayıcıda mı çalışıyoruz?
    // Token'ı localStorage'dan sil
    localStorage.removeItem('authToken');
  }
  this.isAuthenticatedSubject.next(false);
  return this.http.post(`${this.apiUrl}/logout`, {}).pipe(
    tap(() => {
      // Çıkış yaptıktan sonra kullanıcıyı yönlendirebiliriz
      this.router.navigate(['/login']);
    })
  );
}

// Token'ı HTTP header'ına eklemek için metod
private getAuthHeaders(): HttpHeaders {
  let headers = new HttpHeaders();
  if (typeof window !== 'undefined') {  // Tarayıcıda mı çalışıyoruz?
    const token = localStorage.getItem('authToken');
    if (token) {
      headers = headers.set('Authorization', `Bearer ${token}`);  // JWT token'ı Authorization header'ında gönderiyoruz
    }
  }
  return headers;
}

// Token gerektiren korumalı API çağrıları için örnek metod
getProtectedData(): Observable<any> {
  return this.http.get(`${this.apiUrl}/protected`, {
    headers: this.getAuthHeaders()  // Header'da token'ı ekliyoruz
  });
}
}
