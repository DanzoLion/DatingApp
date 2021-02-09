import { HttpClient } from '@angular/common/http';      // HttpClient
import { Injectable } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import { map } from 'rxjs/operators'; // use within pipe() rxjs operator
import { environment } from 'src/environments/environment';
import { User } from '../_models/user'; // response: User
import { PresenceService } from './presence.service';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  //baseUrl = 'https://localhost:5001/api/';                    // baseUrl - used to make requests to our API / keeps app alive as long as it's kept open, or not moved away from
    // baseUrl - used to make requests to our API / keeps app alive as long as it's kept open, or not moved away from
  baseUrl = environment.apiUrl;            // aungular auto-imports this from environment.ts      
                                                                                        // ReplaySubject<User>(1); single value stored, and type of value is User (1) is also the size of our buffer
  private currentUserSource = new ReplaySubject<User>(1);     // observable created here to store our user in // ReplaySubject stores values inside here and emits last value subscribed // or number of values we want to emit
  currentUser$ = this.currentUserSource.asObservable();           // we have access to this property and allows us to set users Main image in the NavBar  // we now set this in nav.component.html

  constructor(private http: HttpClient, private presence: PresenceService) { }                   // we inject our http client into our service here  // PresenceService implemented with SignalR

  login(model: any)         // this method receives credentials from login form from NavBar
  {                                                // returns credentials // model contains username and password we are sending up to the server // completes basic structure of a service
    return this.http.post(this.baseUrl + 'account/login', model).pipe(  // everything in here is an rxjs operator ..
    map((response: User) => {            // changed response to User after creating ts interface
      const user = response;                  // we want our user retrieved from the response
    if (user) {
                                                          //  localStorage.setItem('user', JSON.stringify(user));   // populate received  user in local storage within browser, then pass user as a string  // REPLACED by: this.setCurrentUser(user); 
                                    //  this.currentUserSource.next(user); // where we set our current user from the API // replaced with: this.setCurrentUser(user);
    this.setCurrentUser(user);
    this.presence.createHubConnection(user);   // SignalR implentation
      }
    })
    )      
  }

  register(model: any) {               // when we get our user back from the API it will include the users main PhotoUrl  // we have access to that property inside currentUser$
    return this.http.post(this.baseUrl + "account/register", model).pipe(
      map((user: User) => {
        if (user) {
   //       localStorage.setItem("user", JSON.stringify(user));       // removed and added to setCurrentUser method
  //        this.currentUserSource.next(user);            // replaced with: this.setCurrentUser(user);
      this.setCurrentUser(user);          // when we 
      this.presence.createHubConnection(user);   // SignalR implentation
        }
       //  return user;
      })
    )
  }

setCurrentUser(user: User){                     // helper method implemented here                     // this method sets our current user
  user.roles = [];
  const roles = this.getDecodedToken(user.token).role;                // role is the name of the property even if there is more than one       
  Array.isArray(roles) ? user.roles = roles : user.roles.push(roles); // we check if role is an arry [many] or if not : then user.roles.push to (roles) array ¬ and then we set item as normal below
  localStorage.setItem("user", JSON.stringify(user));    // moved from register above to setCurrentUser
  this.currentUserSource.next(user);      // we set our current user source here
}          

logout() {
  localStorage.removeItem('user');
  this.currentUserSource.next(null);
  this.presence.stopHubConnection();         // SignalR implementation
}

getDecodedToken(token) {
  return JSON.parse(atob(token.split('.')[1]));               // we reference the middle part of the token here, ie the PayLoad // implementation of user roles and retrieving token data
}
}

