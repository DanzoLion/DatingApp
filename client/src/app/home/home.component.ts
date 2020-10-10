import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  registerMode = false;
                                                                                                                                                                    //users:  any;


  constructor(/*private http: HttpClient*/) { }

  ngOnInit(): void {
                                                                            // removed:  this.getUsers();                                                // NB: we need this method to initialise getUsers() function below
  }

registerToggle(){
  this.registerMode = !this.registerMode;
}

/*getUsers() {
  this.http.get("https://localhost:5001/api/users").subscribe(users => this.users = users);  
  //address of API //subscription added, goes to users - this.users - users //array of users, set to users property, property set to user returned
}*/

cancelRegisterMode(event: boolean){           // this will allow us to set (cancelRegister) in home.component.html
  this.registerMode = event;
}


}
