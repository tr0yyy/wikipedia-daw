import { Component, Inject} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { DatePipe } from '@angular/common';
import { environment } from 'src/environments/environment';
import { ActivatedRoute } from '@angular/router';
import { AuthenticationService } from '../services/authentication.service';
import { RoleEnum } from '../models/role.enum';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  articole: ArticolInterface[] = [];
  public articoleFiltrate: ArticolInterface[] = [];

  constructor(http: HttpClient, private datepipe: DatePipe, private authenticationService: AuthenticationService) {
    console.log(environment.apiPath + '/articol')
    if(this.accessToTable) {
      http.get<ArticolInterface[]>(environment.apiPath + '/articol/first50').subscribe(async result => {
        
        this.articole = await result;
        for (var articol of this.articole) {
          articol.link = this.generateLinkFromTitle(window.location.pathname, articol.titlu)
          articol.data_adaugarii = this.datepipe.transform(articol.data_adaugarii as string, 'dd/MM/yyyy') as string
        }
        this.articoleFiltrate = this.articole;
      }, error => console.error(error));
    }
  }

  filterValues(value: any) {
    value = value.target.value
    value = value.toLowerCase();
    if (value == "") {
      this.articoleFiltrate = this.articole;
    } else {
      this.articoleFiltrate = this.articoleFiltrate.filter((articol) => articol.titlu.toLowerCase().includes(value))
    }
  }

  generateLinkFromTitle(baseUrl: string, title: string) {
    let link: string = baseUrl + "articol/" + title;
    return link;
  }

  public accessToTable : boolean = this.checkAccessToTable()

  checkAccessToTable() : boolean {
    let value = this.authenticationService.getRoles()?.includes(RoleEnum.Admin)
    if(value === true) {
      return true;
    } else {
      return false;
    }
  }
}


interface ArticolInterface {
  Id: number;
  domeniu: string;
  titlu: string;
  autor_creare: object;
  data_adaugarii: string;
  continut: string;
  protejat: boolean;
  link: string;
}
