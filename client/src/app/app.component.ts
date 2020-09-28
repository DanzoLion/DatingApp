import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent  implements OnInit{
  title = 'The Dating App';
  users: any;

  constructor(private http: HttpClient) {}
  ngOnInit() {
    this.getUsers();
  }


getUsers() {
  this.http.get('https://localhost:5001/api/users').subscribe(response =>{                  // contains the response from our users API
  this.users = response;                                                                                            // set users: any; class property to this.users = reponse // ie users response
}, error => {
  console.log(error);
})
}
}

