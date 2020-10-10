import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import {HttpClientModule} from '@angular/common/http'
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';    // imported from ngx bootstrap: components

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NavComponent } from './nav/nav.component';
import { FormsModule } from '@angular/forms';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';


@NgModule({                                                           // root module
  declarations: [
    AppComponent,
    NavComponent,
    HomeComponent,
    RegisterComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
   FormsModule,
   BsDropdownModule.forRoot()                             // added from ngx bootstrap: components //.forRoot() means it has services and components it needs to initialise along with root module

  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
