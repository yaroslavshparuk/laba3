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
   selectedYear;
   selectedMonth;
  years = getYears();
  monthNames = ["January", "February", "March", "April", "May", "June",
  "July", "August", "September", "October", "November", "December"
];

constructor(private dataService: DataService) {}

loadDataHandler() {
  this.dataService.loadData()
}

buildDataHandler() {
  this.dataService.buildUserWorks()
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
