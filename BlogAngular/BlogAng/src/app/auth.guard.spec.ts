import { TestBed } from '@angular/core/testing';
import { Router } from '@angular/router';
import { AuthGuard } from './auth.guard';
import { AuthService } from './services/auth.service';

describe('AuthGuard', () => {
  let authGuard: AuthGuard;
  let authService: jasmine.SpyObj<AuthService>;
  let router: jasmine.SpyObj<Router>;

  beforeEach(() => {
    const authServiceSpy = jasmine.createSpyObj('AuthService', ['checkAuthenticationStatus']);
    const routerSpy = jasmine.createSpyObj('Router', ['navigate']);

    TestBed.configureTestingModule({
      providers: [
        AuthGuard,
        { provide: AuthService, useValue: authServiceSpy },
        { provide: Router, useValue: routerSpy },
      ]
    });

    authGuard = TestBed.inject(AuthGuard);
    authService = TestBed.inject(AuthService) as jasmine.SpyObj<AuthService>;
    router = TestBed.inject(Router) as jasmine.SpyObj<Router>;
  });

  it('should allow navigation if user is authenticated', () => {
    authService.checkAuthenticationStatus.and.returnValue(true);

    const canActivate = authGuard.canActivate(
      {} as any, // Mocked ActivatedRouteSnapshot
      {} as any  // Mocked RouterStateSnapshot
    );

    expect(canActivate).toBeTrue();
    expect(router.navigate).not.toHaveBeenCalled();
  });

  it('should redirect to /login if user is not authenticated', () => {
    authService.checkAuthenticationStatus.and.returnValue(false);

    const canActivate = authGuard.canActivate(
      {} as any,
      {} as any 
    );

    expect(canActivate).toBeFalse();
    expect(router.navigate).toHaveBeenCalledWith(['/login']);
  });
});
