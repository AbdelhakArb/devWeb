import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GestionListe } from './gestion-liste';

describe('GestionListe', () => {
  let component: GestionListe;
  let fixture: ComponentFixture<GestionListe>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GestionListe],
    }).compileComponents();

    fixture = TestBed.createComponent(GestionListe);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
