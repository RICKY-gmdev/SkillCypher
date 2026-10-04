import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class RecruiterDashboard {
  private baseUrl = 'http://localhost:5270/api/recruiter';
  constructor(private http: HttpClient) {}

  getRecruiterDashboard(): Observable<any>
  {
    return this.http.get(`${this.baseUrl}/dashboard`)
  }
}
