import { Component } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {
  dayz = getDaysInMonth(11, 2020);
  rows: Row[] =[ {
    UserName: "yaroslav",
    WorkItems: [
      { Id:1, StartDay:1,Duration: 14,Name:"task1" },
      { Id:2, StartDay:11,Duration: 5,Name:"task2" },
      { Id:3, StartDay:15,Duration: 7,Name:"task3" }
    ]
  },
  {
    UserName: "vasya",
    WorkItems: [
      { Id:4, StartDay:1,Duration: 8,Name:"task4" },
      { Id:5, StartDay:9,Duration: 11,Name:"task5" },
      { Id:6, StartDay:22,Duration: 8,Name:"task6" }
    ]
  }
];
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