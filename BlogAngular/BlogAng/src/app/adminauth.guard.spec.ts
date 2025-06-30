import { TestBed } from '@angular/core/testing';
import { AdminAuthGuard } from './adminauth.guard';
import { Router } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';

describe('AdminAuthGuard', () => {
  let guard: AdminAuthGuard;
  let routerSpy: jasmine.SpyObj<Router>;

  beforeEach(() => {
    const routerMock = jasmine.createSpyObj('Router', ['navigate']);

    TestBed.configureTestingModule({
      imports: [RouterTestingModule],
      providers: [
        AdminAuthGuard,
        { provide: Router, useValue: routerMock },
      ],
    });

    guard = TestBed.inject(AdminAuthGuard);
    routerSpy = TestBed.inject(Router) as jasmine.SpyObj<Router>;
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });

  it('should allow activation if admin_token exists', () => {
    // Token varmış gibi yerleştiriyoruz
    spyOn(localStorage, 'getItem').and.returnValue('fake_admin_token');

    const canActivate = guard.canActivate();

    expect(canActivate).toBeTrue();
    expect(routerSpy.navigate).not.toHaveBeenCalled();
  });

  it('should redirect to /admin/login if admin_token does not exist', () => {
    // Token yokmuş gibi yerleştiriyoruz
    spyOn(localStorage, 'getItem').and.returnValue(null);

    const canActivate = guard.canActivate();

    expect(canActivate).toBeFalse();
    expect(routerSpy.navigate).toHaveBeenCalledWith(['/admin/login']);
  });
});
