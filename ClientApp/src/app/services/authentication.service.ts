import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { AccountClient } from '../account/account.client';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  private tokenKey = '';

  constructor(private accountClient: AccountClient, private router: Router) { }

  public login(username: string, password: string) {
    this.accountClient.login(username, password).subscribe((token) => {
      localStorage.setItem(this.tokenKey, token);
      this.router.navigate(['/']);
    });
  };

  public register(email: string, username: string, password: string) {
    this.accountClient.register(email, username, password).subscribe((token) => {
      localStorage.setItem(this.tokenKey, token);
      this.router.navigate(['/']);
    });
  };

  public logout() {
    localStorage.removeItem(this.tokenKey);
    this.router.navigate(['/login']);
  }

  public isLoggedIn() : boolean {
    let token = localStorage.getItem(this.tokenKey);
    return token != null && token.length > 0
  }

  public getToken(): string | null {
    if(this.isLoggedIn()) {
      return localStorage.getItem(this.tokenKey);
    } else {
      return null;
    }
  }
}
