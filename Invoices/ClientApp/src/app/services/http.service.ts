import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../models/user';
@Injectable({
  providedIn: 'root'
})

export class HttpService {
  baseUrl;
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
  }

  buildUserWorks() {
    return this.http.get(this.baseUrl + 'build');
  }
  loadWorkItems(){
    return this.http.get(this.baseUrl + 'load')
  }
}



