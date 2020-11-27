import { Component } from '@angular/core';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
 export class NavMenuComponent {
  years = getYears();
  monthNames = ["January", "February", "March", "April", "May", "June",
  "July", "August", "September", "October", "November", "December"
];
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
