import { Injectable, inject } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import { ApiService } from './api.service';
import { TokenStorageService } from './token-storage.service';
import { 
  AuthResponse,
  SignInRequest,
  SignUpRequest,
  UserDto 
} from '../models/auth.models';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly api = inject(ApiService);
  private readonly tokenStorage = inject(TokenStorageService);

  private readonly currentUserSubject: BehaviorSubject<UserDto | null>;
  readonly currentUser$: Observable<UserDto | null>;
  readonly isAuthenticated$: Observable<boolean>;

  public constructor() {
    const stored = this.tokenStorage.getUser() as UserDto | null;
    this.currentUserSubject = new BehaviorSubject<UserDto | null>(stored);
    this.currentUser$ = this.currentUserSubject.asObservable();
    this.isAuthenticated$ = this.currentUser$.pipe(map(user => user !== null));
  }

  public signUp(request: SignUpRequest): Observable<AuthResponse> {
    return this.api
      .post<AuthResponse, SignUpRequest>('/auth/signup', request)
      .pipe(tap(response => this.persistSession(response)));
  }

  public signIn(request: SignInRequest): Observable<AuthResponse> {
    return this.api
      .post<AuthResponse, SignInRequest>('/auth/signin', request)
      .pipe(tap(response => this.persistSession(response)));
  }

  public signOut(): void {
    this.tokenStorage.clear();
    this.currentUserSubject.next(null);
  }

  public isAuthenticated(): boolean {
    return this.tokenStorage.getToken() !== null;
  }

  private persistSession(response: AuthResponse): void {
    this.tokenStorage.setToken(response.token);
    this.tokenStorage.setUser(response.user);
    this.currentUserSubject.next(response.user);
  }
}
