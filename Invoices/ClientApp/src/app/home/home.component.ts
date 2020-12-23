import { ChangeDetectionStrategy, Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { DialogTaskViewComponent } from '../dialog-task-view/dialog-task-view';
import { DataService } from '../services/data.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class HomeComponent {
  constructor(private dataService: DataService, public dialog: MatDialog) {}
  dayz = getDaysInMonth(11, 2020);
  userWorks$ = this.dataService.getCreatedReport();
  openDialog(id:number, title: string) {
    this.dialog.open(DialogTaskViewComponent, {
      data: {
        Id : id,
        Title: title
      }
    });
  }
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
