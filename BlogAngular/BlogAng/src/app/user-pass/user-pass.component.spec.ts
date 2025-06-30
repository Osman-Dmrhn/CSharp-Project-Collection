import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserPassComponent } from './user-pass.component';

describe('UserPassComponent', () => {
  let component: UserPassComponent;
  let fixture: ComponentFixture<UserPassComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UserPassComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserPassComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
