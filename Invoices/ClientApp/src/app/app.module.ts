import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import {  HomeComponent } from './home/home.component';
import { HttpService } from './services/http.service';
import { DataService } from './services/data.service';
import {MatDialogModule} from '@angular/material/dialog';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatButtonModule } from '@angular/material/button';
import { MAT_FORM_FIELD_DEFAULT_OPTIONS } from '@angular/material/form-field';
import { DialogTaskViewComponent } from './dialog-task-view/dialog-task-view';
@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    DialogTaskViewComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    MatDialogModule,
    MatButtonModule,
    BrowserAnimationsModule,
    ReactiveFormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' }
    ])
  ],
  providers: [
    { provide: MAT_FORM_FIELD_DEFAULT_OPTIONS, useValue: { appearance: 'fill' }  },
    HttpService, DataService
  ],
  bootstrap: [AppComponent],
  entryComponents: [HomeComponent, DialogTaskViewComponent]
})
export class AppModule { }
