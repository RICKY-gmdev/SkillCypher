import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Skill {
  skillId: number;
  skillName: string;
}

@Injectable({
  providedIn: 'root',
})
export class SkillsService {
  private baseUrl = 'http://localhost:5270/api/skills';

  constructor(private http: HttpClient) {}

  getSkills(): Observable<Skill[]> {
    return this.http.get<Skill[]>(this.baseUrl);
  }
}
