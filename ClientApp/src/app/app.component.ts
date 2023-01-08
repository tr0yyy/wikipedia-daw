import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { SpinnerService } from './helpers/spinner.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent {
  constructor(public spinnerService: SpinnerService,
    private httpClient: HttpClient) { }
  title = 'app';
}
