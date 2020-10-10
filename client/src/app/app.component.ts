import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { User } from './_models/user';
import { AccountService } from './_services/account.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent  implements OnInit{
  title = 'The Dating App';
  users: any;

  constructor(/*private http: HttpClient,*/ private accountService: AccountService) {}                  // we bring in our App accountService // functionality removed and added to home.component.ts
  
  ngOnInit() {
                                                                                                                                                  //remved with getUsers() // this.getUsers();
    this.setCurrentUser();                                                                                                          // calls the method .setCurrentUser() when we initialise component ngOnInit()
  }

  setCurrentUser(){
    const user: User = JSON.parse(localStorage.getItem('user'));                                          // .parse() is used to remove the object from its stringified form // access user from localStorage
    this.accountService.setCurrentUser(user);
  }

/*getUsers() {
  this.http.get('https://localhost:5001/api/users').subscribe(response =>{                          // contains the response from our users API
  this.users = response;                                                                                                       // set users: any; class property to this.users = reponse // ie users response
}, error => {
  console.log(error);
})

}*/

}

