import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { AccountService } from '../_services/account.service';
import { User } from '../_models/user';
import { take } from 'rxjs/operators';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {

  constructor(private accountService: AccountService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    let currentUser: User;                                                                                                                                              // can't use const as it must always be initialized firts, insead we use let
   // if unsure as to whether or not to unsubscribe to something we add a pipe method
 // user => set to currentUser variable we set in our pipe() chain // will contain the contents of that current user or it will be NULL
   this.accountService.currentUser$.pipe(take(1)).subscribe(user => currentUser = user);                                      // we complete after extracting (1) item ie after receiving a single current user // once completed we are effectively unsubscribed
   if (currentUser) { // here we clone the request and add our authentication onto the clone
    request = request.clone({
      setHeaders: { // attaches every token for every request when we are logged in and sends up with our request
        Authorization: `Bearer ${currentUser.token}`                                                            // `backticks` used here to allow us to concatenate directly inside the same string // we need a space after bearer
      }
    })
   }

    return next.handle(request);      // because we have clone our request when we return here the request when we are logged in will receive the authorisation header and send up with our request
  }
}
