import { Component, Inject } from "@angular/core";
import { MAT_DIALOG_DATA } from "@angular/material/dialog";

export interface DialogData {
    Id: number
    Title: string
  }

@Component({
    selector: 'dialog-task-view',
    templateUrl: './dialog-task-view.html' 
    })
    export class DialogTaskViewComponent {
      constructor(@Inject(MAT_DIALOG_DATA) public data: DialogData) {}
    }