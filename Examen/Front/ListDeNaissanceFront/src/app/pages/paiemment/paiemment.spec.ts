import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Paiemment } from './paiemment';

describe('Paiemment', () => {
  let component: Paiemment;
  let fixture: ComponentFixture<Paiemment>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Paiemment],
    }).compileComponents();

    fixture = TestBed.createComponent(Paiemment);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
