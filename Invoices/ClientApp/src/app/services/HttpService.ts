import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map, filter, switchMap, flatMap } from 'rxjs/operators';
import { UserWork } from '../models/UserWork';
import { User } from '../models/user';
@Injectable({
  providedIn: 'root'
})

export class HttpService {
  baseUrl;
  work: UserWork[]
  users: Observable<User[]>;
  result;
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
  }

  getUserWorks() {
    return this.http.get(this.baseUrl + 'build');
  }
}



