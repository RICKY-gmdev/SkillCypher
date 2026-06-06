import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Job {
  jobId: number;
  title: string;
  minSalary: number | null;
  maxSalary: number | null;
  companyId: number;
  companyName: string;
}

export interface JobResponse {
  totalCount: number;
  jobs: Job[];
}

@Injectable({
  providedIn: 'root',
})
export class Jobs {
  private baseUrl = 'http://localhost:5270/api/jobs';

  constructor(private http: HttpClient) { }

  getJobs(page = 1, pageSize = 10, search = '', location = ''): Observable<JobResponse> {
    const params = new HttpParams()
      .set('page', page)
      .set('pageSize', pageSize)
      .set('search', search)
      .set('location', location);

    return this.http.get<JobResponse>(this.baseUrl, { params });
  }

  getJobById(id: number): Observable<any> {
    return this.http.get(`${this.baseUrl}/${id}`);
  }
}
