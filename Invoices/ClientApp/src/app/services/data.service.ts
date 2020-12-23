import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { HttpService } from './http.service';
@Injectable({
  providedIn: 'root'
})

export class DataService {
    dataSubject$ = new BehaviorSubject(null)
  constructor(private httpService: HttpService) {}

   getCreatedReport() {
    return this.dataSubject$.asObservable();
}

loadData(selectedYear:number, selectedMonth: string){
    this.httpService.loadWorkItems(selectedYear, selectedMonth).subscribe(error => console.error(error));
}

createReports(selectedYear:number, selectedMonth: string) {
    this.httpService.createReports(selectedYear, selectedMonth).subscribe(data => {
        this.dataSubject$.next(data)
    })
}
}



