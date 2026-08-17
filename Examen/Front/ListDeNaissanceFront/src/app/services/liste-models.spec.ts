import { TestBed } from '@angular/core/testing';

import { ListeModels } from './liste-models';

describe('ListeModels', () => {
  let service: ListeModels;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ListeModels);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
