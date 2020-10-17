import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
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
 constructor(public accountService: AccountService, private router: Router, private toastr: ToastrService) { }  // toastr added // needs to become public so we can access the service within our template       // here we inject our service into this component

  ngOnInit(): void {
                                                                                                                                             // this.currentUser$ = this.accountService.currentUser$;  //removed:  this.getCurrentUser();   // retrieves the current user from the account service
  }

login() {
  this.accountService.login(this.model).subscribe(response => {                    // here we use our account service to login //observable provided here, lazy so we need to subscribe
  this.router.navigateByUrl("/members");                                                       //removed: console.log(response); // replaced with ("/members") to route to members area
                                                                                                                      // removed:  this.loggedIn = true;              // we have a defined logged in variable
  },/* error => {                                                                                                 // http response will be contained inside error here
    console.log(error);
    this.toastr.error(error.error);                                                                              // this informs of error from API server, the error message is contained inside error property here // will provide the reason the error failed
  }*/)}

logout()
{
  this.accountService.logout();
                                                                                                      //removed: this.loggedIn = false;   // simply logged in property set to false // this.loggedIn property will be used to display conditionals // components
 this.router.navigateByUrl("/");                                                    // logs user out to homepage
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
