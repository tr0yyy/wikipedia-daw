import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  public articole: ArticolInterface[] = [];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<ArticolInterface[]>(baseUrl + 'articol').subscribe(result => {
      console.log("test");
      console.log(result);
      this.articole = result;
    }, error => console.error(error));
  }
}


interface ArticolInterface {
  Id: number;
  domeniu: string;
  titlu: string;
  autor_creare: object;
  data_adaugarii: object;
  continut: string;
  protejat: boolean;
}
