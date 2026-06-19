import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VisiteurView } from './visiteur-view';

describe('VisiteurView', () => {
  let component: VisiteurView;
  let fixture: ComponentFixture<VisiteurView>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [VisiteurView],
    }).compileComponents();

    fixture = TestBed.createComponent(VisiteurView);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
