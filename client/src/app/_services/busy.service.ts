import { Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';

@Injectable({
  providedIn: 'root'
})
export class BusyService {
  busyRequestCount = 0;                       // this property increments and decrements as a counter every time a req. is made and completed

  constructor(private spinnerService: NgxSpinnerService)  {  }      // inject service here

  Busy() {                                                      // method called Busy() added here
    this.busyRequestCount++;                    // increment variable
    this.spinnerService.show(undefined, {
      type: 'line-scale-party',                         // choice of spinner we are using
      bdColor: 'rgba(255,255,255,0)',            // background of spinner
      color: '#333333'                                  // spinner colour
    });
  } 
  
  idle() {
    this.busyRequestCount--;                  // decrement feature
    if (this.busyRequestCount <= 0){     // check what our busy request is .. handles if count is less than zero
      this.busyRequestCount = 0;          // re-set count to zero if less than zero   // acts as a safety mechanism
      this.spinnerService.hide();              // remove spinner once completed
    }
  }
}
