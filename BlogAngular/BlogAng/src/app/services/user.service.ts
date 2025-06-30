import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../models/user';
import { ChangePasswordModel } from '../models/ChangePasswordModel';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private apiUrl = `https://localhost:7168/api/user`;
 
  constructor(private http: HttpClient) {}
    
  getUser(): Observable<User> {
    const token = this.getToken();
    return this.http.get<User>(`${this.apiUrl}/user-view`,{
     headers:{
            'Authorization': `Bearer ${token}`
        }
    });
  }


  changePassword(model: ChangePasswordModel): Observable<any> {
    const token = this.getToken();
    return this.http.post<any>(`${this.apiUrl}/edit-pass`, model,{
        headers:{
            'Authorization': `Bearer ${token}`
        }
    });
  }

  editUser(user: User): Observable<any> {
    const token = this.getToken();
    return this.http.post<any>(`${this.apiUrl}/edit-user`, user,{
        headers:{
            'Authorization': `Bearer ${token}`
        }
    });
  }

  getToken(): string {
    return localStorage.getItem('authToken') || '';  // Token'ı localStorage'dan alabilirsiniz
  }
}
