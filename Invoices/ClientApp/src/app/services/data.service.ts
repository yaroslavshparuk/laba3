import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { HttpService } from './http.service';
@Injectable({
  providedIn: 'root'
})

export class DataService {
    dataSubject$ = new BehaviorSubject(null)
  constructor(private httpService: HttpService) {}

   getBuiltUserWorks() {
    return this.dataSubject$.asObservable();
}

loadData(){
    this.httpService.loadWorkItems().subscribe(error => console.error(error));
}

buildUserWorks() {
    this.httpService.buildUserWorks().subscribe(data => {
        this.dataSubject$.next(data)
    })
}
}



