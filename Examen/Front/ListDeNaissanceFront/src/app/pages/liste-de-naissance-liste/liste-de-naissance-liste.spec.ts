import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListeDeNaissanceListe } from './liste-de-naissance-liste';

describe('ListeDeNaissanceListe', () => {
  let component: ListeDeNaissanceListe;
  let fixture: ComponentFixture<ListeDeNaissanceListe>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ListeDeNaissanceListe],
    }).compileComponents();

    fixture = TestBed.createComponent(ListeDeNaissanceListe);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
