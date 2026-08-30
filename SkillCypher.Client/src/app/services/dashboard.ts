import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class Dashboard {
  private baseUrl = 'http://localhost:5270/api/applicant';
  constructor(private http: HttpClient) {}

  getDashBoard(): Observable<any>
  {
    return this.http.get(`${this.baseUrl}/dashboard`);
  }
}
