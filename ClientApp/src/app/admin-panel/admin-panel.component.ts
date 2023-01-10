import { Component } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { AuthenticationService } from '../services/authentication.service';

@Component({
  selector: 'app-admin-panel',
  templateUrl: './admin-panel.component.html'
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
    var roless = this.AuthService.getRoles() // rolurile utilizatorului selectat
    this.afis = true // afisez panoul pentru schimbare de roluri
    this.userAfis = user; // userul selectat
    var roles : string = ""

    if(roless != null)
      for(let ch of roless)
        console.log(ch)


    if(roless != null){
      for(let ch of roless){
        roles = roles.concat(ch)
      }
    }

    // actualizez variabilele cu roluri
    // if(roles != null){
    //   console.log("=============")

    //   for(let rol of roles){
        console.log(roles)
        if(roles == "admin") {
          this.isAdmin = true
          console.log("----------")

        } else if (roles == "moderator") {
          this.isModerator = true
          console.log("----------")

        } else if ( roles == "utilizator") {
          this.isUtilizator = true
          console.log("----------")

        }
    //   }
    // }
    console.log("==============")
    console.log(this.isAdmin)
    console.log(this.isModerator)
    console.log(this.isUtilizator)
    
    console.log(user.username)
    console.log(roles?.length)
    console.log('Click!')

    this.search("") // resetez search boxul de utilizatori
  }
  changeAdmin(){
    this.isAdmin = !this.isAdmin
    console.log(this.isAdmin)
  }
  changeModerator(){
    this.isModerator = !this.isModerator
    console.log(this.isAdmin)
  }
  changeUtilizator(){
    this.isUtilizator = !this.isUtilizator
    console.log(this.isAdmin)
  }





}
interface user {
  roles: [];
  username: string;
}
