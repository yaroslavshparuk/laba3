import { ChangeDetectionStrategy, Component } from '@angular/core';
import { DataService } from '../services/data.service';
import { HttpService } from '../services/http.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
 export class NavMenuComponent {
   years = getYears();
   monthNames = ["January", "February", "March", "April", "May", "June",
   "July", "August", "September", "October", "November", "December"
  ];
  selectedYear = this.years[0]
  selectedMonth = this.monthNames[0]

constructor(private dataService: DataService) {}

loadDataHandler() {
  this.dataService.loadData(this.selectedYear,this.selectedMonth)
}

createReportsHandler() {
  this.dataService.createReports(this.selectedYear,this.selectedMonth)
}

setSelectedYear(year:number) : void {
  this.selectedYear = year;
}
setSelectedMonth(month:string) : void {
  this.selectedMonth = month;
}

}
function getYears()
{
  var fullYear = new Date().getFullYear();
  var years = [];
 while(fullYear != 2010){
   years.push(fullYear);
   fullYear = fullYear - 1;
 }
 return years;
}

