import { TestBed } from '@angular/core/testing';

import { ListeNaissance } from './liste-naissance';

describe('ListeNaissance', () => {
  let service: ListeNaissance;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ListeNaissance);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
