import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreerListe } from './creer-liste';

describe('CreerListe', () => {
  let component: CreerListe;
  let fixture: ComponentFixture<CreerListe>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreerListe]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreerListe);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
