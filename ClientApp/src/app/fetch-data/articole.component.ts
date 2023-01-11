import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';
import { environment } from 'src/environments/environment';
import { DatePipe } from '@angular/common';
import { Sort } from '@angular/material/sort';

@Component({
  selector: 'app-articole',
  templateUrl: './articole.component.html'
})
export class ArticoleComponent {
  public listaArticole: ArticolInterface[] = [];
  public sortedData: ArticolInterface[] = [];

  constructor(http: HttpClient, private route: ActivatedRoute, private datepipe: DatePipe) {
    console.log(this.route.snapshot.paramMap.get('domeniu'))
    http.get<ArticolInterface[]>(environment.apiPath + '/articol/articole-domeniu/' + this.route.snapshot.paramMap.get('domeniu')).subscribe(result => {
      this.listaArticole = result;
      for (var articol of this.listaArticole) {
        articol.data_adaugarii = this.datepipe.transform(articol.data_adaugarii as string, 'dd/MM/yyyy') as string
      }
      this.sortedData = this.listaArticole.slice()
      console.log(this.listaArticole)
    }, error => console.error(error));
  }

  generateLinkFromTitle(title: string) {
    let link: string = window.location.origin + '/articol/' + title;
    return link;
  }

  sortData(sort: Sort) {
    const data = this.listaArticole.slice();
    if (!sort.active || sort.direction === '') {
      this.sortedData = data;
      return;
    }

    console.log(sort.active)

    this.sortedData = data.sort((a, b) => {
      const isAsc = sort.direction === 'asc';
      switch (sort.active) {
        case 'name':
          return compare(a.titlu, b.titlu, isAsc);
        case 'user':
          return compare(a.user, b.user, isAsc);
        case 'data_adaugarii':
          return compare(a.data_adaugarii, b.data_adaugarii, isAsc);
        default:
          return 0;
      }
    });
  }

}
function compare(a: number | string, b: number | string, isAsc: boolean) {
  return (a < b ? -1 : 1) * (isAsc ? 1 : -1);
}


interface ArticolInterface {
  Id: number;
  domeniu: string;
  titlu: string;
  user: string;
  data_adaugarii: string;
  continut: string;
  protejat: boolean;
  link: string;
}