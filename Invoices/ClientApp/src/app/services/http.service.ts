import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
@Injectable({
  providedIn: 'root'
})

export class HttpService {
  baseUrl;
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
  }

  createReports(selectedYear:number, selectedMonth: string) {
    return this.http.get(this.baseUrl + 'report/' + selectedYear + '/' + selectedMonth);
  }
  loadWorkItems(selectedYear:number, selectedMonth: string){
    return this.http.get(this.baseUrl + 'load/' + selectedYear + '/' + selectedMonth)
  }
}



