import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AccountService } from '../_services/account.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {                                                                                           // the guard will automatically subscribe to our current user as it is set-up in account.service.ts
  constructor(private accountService: AccountService, private toastr: ToastrService)  { }                                    // we need a constructor so we can inject our account.service.ts here 
                                                                                      // because we are inside a router AuthGuard it will handle the subscription so we need to program the canActivate() function below:
  canActivate(): Observable<boolean> {                      // these are the items that can be returned from our guard // list // removed all other elements and will only return a boolen true or false
    return this.accountService.currentUser$.pipe(       // here we will perform an action where we don't need to subscribe .. so we can use pipe() and map()
      map(user => {                                                         // user where currentUser$ is returned
        if (user) return true;                                              // here we perform our check to see if we have a user after it is piped and mapped // no error as this is an observable of true that matches our boolean observable <boolean>
        this.toastr.error("You shall not pass!")              // if not user, then implement toastr.error
      })                                                                            // this will now protect our routes, and next we configure our routing configurations
    )
  }
  
}
