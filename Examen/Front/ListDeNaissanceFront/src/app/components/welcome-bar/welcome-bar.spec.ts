import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WelcomeBar } from './welcome-bar';

describe('WelcomeBar', () => {
  let component: WelcomeBar;
  let fixture: ComponentFixture<WelcomeBar>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [WelcomeBar],
    }).compileComponents();

    fixture = TestBed.createComponent(WelcomeBar);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
