import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from '../_models/user';
import { AccountService } from '../_services/account.service';    // AccountService

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
model: any = {}
// currentUser$: Observable<User>; //account service already has this as a property in useage so this can be removed      //removed: loggedIn: boolean;



  constructor(public accountService: AccountService) { }                     // needs to become public so we can access the service within our template       // here we inject our service into this component

  ngOnInit(): void {
 // this.currentUser$ = this.accountService.currentUser$;                       //removed:  this.getCurrentUser();   // retrieves the current user from the account service
  }

login() {
  this.accountService.login(this.model).subscribe(response => {                                                     // here we use our account service to login //observable provided here, lazy so we need to subscribe
    console.log(response);
                                                                                                                      // removed:  this.loggedIn = true;              // we have a defined logged in variable
  }, error => {
    console.log(error);
  })                                                                                         
}

logout()
{
  this.accountService.logout();
                                                                                                      //removed: this.loggedIn = false;   // simply logged in property set to false // this.loggedIn property will be used to display conditionals // components
}
/*  // this method is now removed as we are now retrieving from the account service
getCurrentUser() {
  this.accountService.currentUser$.subscribe(user => {
    this.loggedIn = !!user;                                                             // !! turns our object into a boolean where !! is null, or is true
  }, error => {
    console.log(error);                                                         // error management implemented here if there are any errors
  })
}*/


}
