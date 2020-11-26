import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';

@Component({
  selector: 'app-build-invoice',
  templateUrl: './build-invoice.component.html',
  styleUrls: ['./build-invoice.component.css']
})
export class BuildInvoiceComponent {

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get(baseUrl + 'build').subscribe(error => console.error(error));
  }

}
