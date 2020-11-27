import { Component, OnInit } from '@angular/core';
import { User } from '../models/user';
import { UserWork } from '../models/UserWork';
import { HttpService } from '../services/HttpService';

@Component({
  selector: 'app-build-invoice'
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


