import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from './services/auth.service'; // AuthService'yi kullanarak JWT kontrolü yapacağız

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private authService: AuthService, private router: Router) {}

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
      if (this.authService.checkAuthenticationStatus()) {
        return true;
      } else {
        // Kullanıcı giriş yapmamışsa login sayfasına yönlendir
        this.router.navigate(['/login']);
        return false;
      }
  }
}
