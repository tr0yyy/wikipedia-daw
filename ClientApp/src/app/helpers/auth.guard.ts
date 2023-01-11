import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { RoleEnum } from '../models/role.enum';
import { AuthenticationService } from '../services/authentication.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private authService: AuthenticationService, private router: Router) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
      console.log(state.url);
      if(state.url == '/admin-panel' && !this.authService.getRoles()?.includes(RoleEnum.Admin)) {
        this.router.navigate(['/login']);
      }
      if (!this.authService.isLoggedIn()) {
        this.router.navigate(['/login']);
      }
      return true;
  }
  
}
