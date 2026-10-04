import { TestBed } from '@angular/core/testing';

import { RecruiterDashboard } from './recruiter-dashboard';

describe('RecruiterDashboard', () => {
  let service: RecruiterDashboard;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(RecruiterDashboard);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
