import { Component } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-admin-panel',
  templateUrl: './admin-panel.component.html'
})
export class AdminPanelComponent {
  public utilizatori !: users[];
  public utilizatoriFiltrati : users[] = [];

  constructor(private http: HttpClient) {
    this.http.get<users[]>(environment.apiPath + "/accounts/users/all").subscribe(async result => {
      this.utilizatori = await result;
      // console.log(this.utilizatori);

    })
  }
  
  

  search(value: string) {
    this.utilizatoriFiltrati = []
    for(let utilizator of this.utilizatori){
      if(utilizator.username.includes(value)){
        this.utilizatoriFiltrati.push(utilizator)
    }
    console.log(this.utilizatoriFiltrati)
  }
}

}
interface users {
  roles: [];
  username: string;
}
