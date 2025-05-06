import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject, Observable, tap, catchError, throwError } from 'rxjs';
import { LoginUser } from '../core/models/login-user';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly API_URL = 'https://localhost:7128/api/Auth';
  private isLoggedInSubject = new BehaviorSubject<boolean>(false);
  public isLoggedIn$ = this.isLoggedInSubject.asObservable();

  constructor(
    private http: HttpClient,
    private router: Router
  ) {
    // Initialize login state from cookie
    this.isLoggedInSubject.next(document.cookie.includes('MyCookieAuth'));
  }

  login(loginUser: LoginUser): Observable<any> {

    const headers = new HttpHeaders({
      'Content-Type': 'application/json'
    });

    return this.http.post(`${this.API_URL}/login`, loginUser, {
      headers,
      withCredentials: true,
      responseType: 'text',
      observe: 'response'
    }).pipe(
      tap({
        next: (response) => {
          this.setLoggedIn(true);
        },
        error: (error) => {
          console.error('Error in tap operator:', error);
        }
      }),
      catchError((error: HttpErrorResponse) => {
        console.error('Login error details:');
        return throwError(() => error);
      })
    );
  }

  isAuthenticated(): boolean {
    return this.isLoggedInSubject.value;
  }

  setLoggedIn(value: boolean): void {
    this.isLoggedInSubject.next(value);
  }

  logout(): Observable<string> {
    return this.http.post(`${this.API_URL}/logout`, {}, {
      withCredentials: true,
      responseType: 'text'
    }).pipe(
      tap((response) => {
        console.log('Logout response:', response);
        this.setLoggedIn(false);
      }),
      catchError((error: HttpErrorResponse) => {
        console.error('Logout error:', error);
        this.setLoggedIn(false);
        return throwError(() => error);
      })
    );
  }
}
