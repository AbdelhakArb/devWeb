import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListeModels } from './liste-models';

describe('ListeModels', () => {
  let component: ListeModels;
  let fixture: ComponentFixture<ListeModels>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ListeModels],
    }).compileComponents();

    fixture = TestBed.createComponent(ListeModels);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
