import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  public listaArticole: ListaArticole[] = [];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<ListaArticole[]>(baseUrl + 'listaArticole').subscribe(result => {
      this.listaArticole = result;
    }, error => console.error(error));
  }
}

interface ListaArticole {
  titlu: string;
  autor_creare: string;
  link: string;
}
