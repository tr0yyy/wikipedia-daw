import { Component, Inject} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  articole: ArticolInterface[] = [];
  public articoleFiltrate: ArticolInterface[] = [];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, public datepipe: DatePipe) {
    http.get<ArticolInterface[]>(baseUrl + 'articol').subscribe(async result => {
      this.articole = await result;
      for (var articol of this.articole) {
        articol.link = this.generateLinkFromTitle(baseUrl, articol.titlu)
        articol.data_adaugarii = this.datepipe.transform(articol.data_adaugarii as string, 'dd/MM/yyyy') as string
      }
      this.articoleFiltrate = this.articole;
    }, error => console.error(error));
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
