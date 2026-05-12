import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class ApiService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = environment.apiBaseUrl;

  private handleError(error: HttpErrorResponse) {
    return throwError(() => error);
  }

  public get<T>(path: string): Observable<T> {
    return this.http
      .get<T>(`${this.baseUrl}${path}`)
      .pipe(catchError(this.handleError));
  }

  public post<TResponse, TBody = unknown>(path: string, body: TBody): Observable<TResponse> {
    return this.http
      .post<TResponse>(`${this.baseUrl}${path}`, body)
      .pipe(catchError(this.handleError));
  }
}
