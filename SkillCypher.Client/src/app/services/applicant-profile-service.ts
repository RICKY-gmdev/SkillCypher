import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ApplicantProfileService {
  private baseUrl = 'http://localhost:5270/api/applicant';

  constructor(private http: HttpClient) { }

  getProfile(): Observable<any> {
    return this.http.get(`${this.baseUrl}/profile`);
  }

  addSkill(skillId: number): Observable<any> {
    return this.http.post(`${this.baseUrl}/skills`, { skillId });
  }

  syncSkills() : Observable<any>
  {
    return this.http.post(`${this.baseUrl}/skills/sync`, {});
  }
}
