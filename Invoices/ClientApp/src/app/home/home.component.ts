import { ChangeDetectionStrategy, Component } from '@angular/core';
import { DataService } from '../services/data.service';
import { HttpService } from '../services/http.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class HomeComponent {
  constructor(private dataService: DataService) {}
  dayz = getDaysInMonth(11, 2020);
  userWorks$ = this.dataService.getBuiltUserWorks();
}

function getDaysInMonth(month, year) {
  var date = new Date(year, month, 1);
  var days = [];
  while (date.getMonth() === month) {
    days.push(new Date(date).getDate()) ;
    date.setDate(date.getDate() + 1);
  }
  return days;
}

interface Row{
  UserName:string;
  WorkItems:WorkItem[];
}

interface WorkItem {
  Id: number;
  StartDay: number;
  Duration: number;
  Name:string;
}