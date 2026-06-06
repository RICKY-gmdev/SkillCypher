import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class Auth {
  private baseUrl = 'http://localhost:5270/api/auth';
  constructor(private http: HttpClient) {}

  login(email: string, password: string): Observable<any>{
    return this.http.post(`${this.baseUrl}/login`,{email,password});
  }

  register(name:string,email:string,password:string,role:string):Observable<any>{
    return this.http.post(`${this.baseUrl}/register`,{name,email,password,role});
  }

  saveToken(token: string):void
  {
    localStorage.setItem('token',token);
  }

  getToken(): string| null{
    return localStorage.getItem('token');
  }
  logout(): void{
    localStorage.removeItem('token');
  }

  isLoggedIn(): boolean{
    return !!this.getToken();
  }
}