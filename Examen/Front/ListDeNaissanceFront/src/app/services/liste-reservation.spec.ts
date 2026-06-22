import { TestBed } from '@angular/core/testing';

import { ListeReservation } from './liste-reservation';

describe('ListeReservation', () => {
  let service: ListeReservation;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ListeReservation);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
