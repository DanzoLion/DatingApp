import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { NavigationExtras, Router } from '@angular/router';
import { Toast, ToastrService } from 'ngx-toastr';
import { catchError } from 'rxjs/operators';
import { UseExistingWebDriver } from 'protractor/built/driverProviders';
import { Key } from 'protractor';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private router: Router, private toastr: ToastrService) {}             // for certain responses we will re-direct to an error page // for ToastrService we might want to display a toastr notification 

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> { // just like for any other observable we will need a .pipe() method to add any functionality inside here
    return next.handle(request).pipe(
      catchError(error => {                                                                         // error is going to be the cosole log we saw on the error response we saw on our console log
          if(error) {                                                                                       // where we check to see if there is an error    // we will switch depending on the status of the error
            switch(error.status) {                                                                  // we attempt to catch all the errors within the switch-case statement
              case 400:
                if (error.error.errors){
                  const modalStateErrors = [];                                              // ASP.NET defines these errors as modalStateErrors
                  for (const key in error.error.errors){
                    if (error.error.errors[key]) {
                      modalStateErrors.push(error.error.errors[key])              // the idea is to flatten our error responses from validation error and push them into an array const modalStateErrors[]
                    }
                  }
                  throw modalStateErrors.flat();                   // after for loop we throw modalStateErrors back to the component // will display a list of validation errors below the form // flat will flatten our array of arrys
                } else if (typeof(error.error) === 'object') {    // else will check if this is just a normal 400 error, rather than special case 400 error .. we two 400 errors we need to manage
                  this.toastr.error(error.statusText, error.status);  // test to see if we have an object, if it is it's an error object
                } else {
                  this.toastr.error(error.error, error.status);
                }
              break;
              case 401:
                this.toastr.error(error.statusText, error.status);
                break;
                case 404:
                  this.router.navigateByUrl('/not-found');                        // router navigates to re-direct not-found page   // will add /not-found page we'll redirect to
                  break;
                  case 500:     // again, we re-direct to a server error page; we also want to get deails of the error that gets returned from API; we can use router feature where we pass it a state ie naviagionExtras: NaviagtionExtras (type of NavigationExtras)
                    const navigationExtras: NavigationExtras = {state: {error: error.error}}; // specify the state, and error.error is the exception we get back from our API
                    this.router.navigateByUrl('/server-error', navigationExtras);   // we pass in the state navigationExtras here 
                    break;

            
              default:
                this.toastr.error('Something unexpected went wrong');
                console.log(error);                                                                     // console.log will allow us to see what the error is // provides an opportunity to tweak interceptor if we need to add a new case to take care of occurring errors
                break;
            }                                                                               
          }
          return throwError(error);                                                           // if we can't catch the error we return the original error that was generated in the first instance // shouldn't necessarily hit this part
      })
    )
  }
}
