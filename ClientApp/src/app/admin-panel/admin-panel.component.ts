import { Component } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { AuthenticationService } from '../services/authentication.service';
import { RoleEnum } from '../models/role.enum';

@Component({
  selector: 'app-admin-panel',
  templateUrl: './admin-panel.component.html',
  styleUrls: ['./admin-panel.component.css']
})
export class AdminPanelComponent {
  public utilizatori !: user[];
  public utilizatoriFiltrati : user[] = [];
  public afis : boolean = false;
  public userAfis !: user; // if true, afiseaza setarile de moficare ale utilizatorului
  // public selectedUser = ; // preia vectorul de roluri ale utilizatorului
  // Roluri utilizator
  public isAdmin : boolean = false;
  public isModerator : boolean = false;
  public isUtilizator : boolean = false;




  constructor(private http: HttpClient, private AuthService: AuthenticationService) {
    this.http.get<user[]>(environment.apiPath + "/accounts/users/all").subscribe(async result => {
      this.utilizatori = await result;
      // console.log(this.utilizatori);
      
    })
  }
  
  
  // functia de filtrare al utilizatorilor
  search(value: string) {
    this.utilizatoriFiltrati = []
    if(value == "")
      return
    for(let utilizator of this.utilizatori){
      if(utilizator.username.includes(value)){
        this.utilizatoriFiltrati.push(utilizator)
      }
      console.log(this.utilizatoriFiltrati)
    }
  }

  afiseazaUtilizator(user: user){
    var roless = user.roles // rolurile utilizatorului selectat
    this.afis = true // afisez panoul pentru schimbare de roluri
    this.userAfis = user; // userul selectat
    var roles : string = ""

    for (let rol of user.roles) {
      if(rol == RoleEnum.User) {
        this.isUtilizator = true
      } else if(rol == RoleEnum.Moderator) {
        this.isModerator = true
      } else {
        this.isAdmin = true
      }
    }
        

    this.search("") // resetez search boxul de utilizatori
  }
  changeAdmin(){
    this.isAdmin = !this.isAdmin
    console.log(this.isAdmin)
  }
  changeModerator(){
    this.isModerator = !this.isModerator
    console.log(this.isModerator)
  }
  changeUtilizator(){
    this.isUtilizator = !this.isUtilizator
    console.log(this.isUtilizator)
  }

  saveEdit() {
    var exportRoles = []
    if(this.isAdmin) {
      exportRoles.push(RoleEnum.Admin as string);
    }
    if(this.isModerator) {
      exportRoles.push(RoleEnum.Moderator as string);
    }
    if(this.isUtilizator) {
      exportRoles.push(RoleEnum.User as string);
    }
    this.http.post<string>(environment.apiPath + '/accounts/update-roles', {
      roles: exportRoles,
      username: this.userAfis.username
    }).subscribe({
      next: (result) => {
        window.location.reload();
      },
      error: (error: HttpErrorResponse) => {
        console.log(error);
      }
  });
  }





}
interface user {
  roles: [];
  username: string;
}
