import { HttpClient } from '@angular/common/http';
import { Component, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Route, Router } from '@angular/router';
import { environment } from '../../environments/environment';
import { RoleEnum } from '../models/role.enum';
import { AuthenticationService } from '../services/authentication.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  // string[] articole
  public articole: string[] = [];
  // ---
  public websiteCtrl : FormControl = new FormControl()
  public websiteFilterCtrl : FormControl = new FormControl()


  constructor(private http: HttpClient, private authenticationService: AuthenticationService, private router: Router) {}

  ngOnInit(){
    
  }
  isExpanded = false;

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  userLoggedIn = this.authenticationService.isLoggedIn()

  public logout() {
    this.authenticationService.logout();
  }

  /*SearchBar function*/
  search(value: string) {
    if (value !== "") {
      this.http.get<string[]>(environment.apiPath + "/articol/alltitles/" + value).subscribe(async result => {
        this.articole = await result;
        console.log(this.articole);
        
        let input = document.getElementById('')

      }, error => console.error(error))
    } else {
      this.articole = [];
      console.log(this.articole);

    }
  }
  generateLinkFromTitle(title: string) {
    let link: string = window.location.pathname + "articol/" + title;
    return link;
  }


  public isAdmin: boolean = this.checkIfAdmin()

  checkIfAdmin(): boolean {
    let value = this.authenticationService.getRoles()?.includes(RoleEnum.Admin)
    if (value === true) {
      return true;
    } else {
      return false;
    }
  }

}
