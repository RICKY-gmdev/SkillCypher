import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class MatchService {
  private baseUrl = 'http://localhost:5270/api/match';

  constructor(private http: HttpClient) {}

  getMatchScore(applicantId: number, jobId: number): Observable<any> {
    return this.http.get(`${this.baseUrl}/${applicantId}/${jobId}`);
  }
}