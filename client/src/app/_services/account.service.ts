import { HttpClient } from '@angular/common/http';      // HttpClient
import { Injectable } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import { map } from 'rxjs/operators'; // use within pipe() rxjs operator
import { User } from '../_models/user'; // response: User

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = 'https://localhost:5001/api/';                    // baseUrl - used to make requests to our API / keeps app alive as long as it's kept open, or not moved away from
                                                                                        // ReplaySubject<User>(1); single value stored, and type of value is User (1) is also the size of our buffer
  private currentUserSource = new ReplaySubject<User>(1);     // observable created here to store our user in // ReplaySubject stores values inside here and emits last value subscribed // or number of values we want to emit
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient) { }                   // we inject our http client into our service here 

  login(model: any)         // this method receives credentials from login form from NavBar
  {                                                // returns credentials // model contains username and password we are sending up to the server // completes basic structure of a service
    return this.http.post(this.baseUrl + 'account/login', model).pipe(  // everything in here is an rxjs operator ..
map((response: User) => {            // changed response to User after creating ts interface
  const user = response;                  // we want our user retrieved from the response
  if (user) {
    localStorage.setItem('user', JSON.stringify(user));   // populate received  user in local storage within browser, then pass user as a string
    this.currentUserSource.next(user); // where we set our current user from the API
  }
})
    )      
  }

  register(model: any) {
    return this.http.post(this.baseUrl + "account/register", model).pipe(
      map((user: User) => {
        if (user) {
          localStorage.setItem("user", JSON.stringify(user));
          this.currentUserSource.next(user);
        }
        return user;
      })
    )
  }



setCurrentUser(user: User){                     // helper method implemented here
  this.currentUserSource.next(user);
}          

logout() {
  localStorage.removeItem('user');
  this.currentUserSource.next(null);
}

}
