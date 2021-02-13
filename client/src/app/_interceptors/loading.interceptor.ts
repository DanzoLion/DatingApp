import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { BusyService } from '../_services/busy.service';
import { finalize } from 'rxjs/operators';
import { delay } from 'rxjs/operators';

@Injectable()
export class LoadingInterceptor implements HttpInterceptor {

  constructor(private busyService: BusyService) {}            // imported BusyService we created earlier in busy.service.ts

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    this.busyService.Busy();                                              // here we call our Busy() method
    return next.handle(request).pipe(
      //delay(1000),                                              // delay is now removed at the end of build, as we don't want artificially induced delay in our production build  
      finalize(() => {                                                          // sets service to idle when operation completed
        this.busyService.idle();
      })
    )
    
  }
}
