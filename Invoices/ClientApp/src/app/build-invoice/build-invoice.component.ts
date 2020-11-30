import { Component, OnInit } from '@angular/core';
import { HttpService } from '../services/HttpService';

@Component({
  selector: 'app-build-invoice',
  template: 'build-invoice.component.html'
})
export class BuildInvoiceComponent implements OnInit {
  users;
  httpService: HttpService;
  constructor(httpService: HttpService) { this.httpService = httpService }

  ngOnInit() {
    this.loadUsers()
  }

  loadUsers() {
    this.httpService.getUserWorks()
      .subscribe(((data) => this.users = data));
  }
}


