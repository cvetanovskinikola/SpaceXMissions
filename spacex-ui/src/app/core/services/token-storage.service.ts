import { Injectable } from '@angular/core';

const TOKEN_KEY = 'spacex_auth_token';
const USER_KEY = 'spacex_auth_user';

@Injectable({ providedIn: 'root' })
export class TokenStorageService {
  public getToken(): string | null {
    return localStorage.getItem(TOKEN_KEY);
  }

  public setToken(token: string): void {
    localStorage.setItem(TOKEN_KEY, token);
  }

  public getUser(): unknown {
    const raw = localStorage.getItem(USER_KEY);
    return raw ? JSON.parse(raw) : null;
  }

  public setUser(user: unknown): void {
    localStorage.setItem(USER_KEY, JSON.stringify(user));
  }

  public clear(): void {
    localStorage.removeItem(TOKEN_KEY);
    localStorage.removeItem(USER_KEY);
  }
}
