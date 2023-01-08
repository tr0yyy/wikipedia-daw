import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import jwtDecode from "jwt-decode";
import { AccountClient } from '../account/account.client';
import { Claims } from '../models/claims.enum';
import { User } from '../models/user.model';
import { Result } from './result.interface';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  private userData = '';

  constructor(private accountClient: AccountClient, private router: Router, private snackBar: MatSnackBar) { }

  public login(username: string, password: string) {
    this.accountClient.login(username, password).subscribe({
        next: (result) => {
          console.log(result);
          this.handleLogin(result);
        },
        error: (error: HttpErrorResponse) => {
          this.handleError(error);
        }
    });
  }

  public register(email: string, username: string, password: string) {
    this.accountClient.register(email, username, password).subscribe({
      next: (result) => {
        this.handleLogin(result);
      },
      error: (error: HttpErrorResponse) => {
        this.handleError(error);
      }
  });
  };

  public logout() {
    localStorage.removeItem(this.userData);
    this.router.navigate(['/'])
    .then(() => {
      window.location.reload()
    });
  }

  public isLoggedIn() : boolean {
    let token = this.getUser()?.token
    return token != null && token.length > 0
  }

  public getUser(): User | null {
    const userAsJson = localStorage.getItem(this.userData);
    if(userAsJson) {
      let user: User = JSON.parse(userAsJson);
      return user;
    } else {
      return null;
    }
  }

  public getRoles(): string[] | null {
    const userAsJson = localStorage.getItem(this.userData);
    if(userAsJson) {
      let user: User = JSON.parse(userAsJson);
      console.log(user.roles);
      return user.roles;
    } else {
      return null;
    }
  }

  public getToken(): string | null {
      const user = this.getUser();
      if (user) {
        return user.token;
      } else {
        return null;
      }
  }

  private handleLogin(result: Result<string>) {
    var message : string;
    console.log(result.result);
    if(result.isSuccess && result.result.length > 1) {
      const decodedJwt = jwtDecode<any>(result.result);
      const user = new User(
        decodedJwt[Claims.NameTokenKey],
        decodedJwt[Claims.RoleTokenKey],
        result.result
      );
      localStorage.setItem(this.userData, JSON.stringify(user));
      this.router.navigate(['/']).then(() => {
        window.location.reload()
      });
      message = 'User succesfully logged in.'
    } else {
      message = 'Something went wrong.'
    }

    this.snackBar.open(message, 'Close')
  }

  private handleError(error: HttpErrorResponse) : void {
    let errorsMessage = []
    var errors : string = JSON.parse(JSON.stringify(error.error));
    this.snackBar.open(errors.split('\'')[1], 'Close')
  }
}
