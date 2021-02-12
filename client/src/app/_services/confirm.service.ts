import { Injectable } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { Observable } from 'rxjs';
import { ConfirmDialogComponent } from '../modals/confirm-dialog/confirm-dialog.component';

@Injectable({
  providedIn: 'root'
})
export class ConfirmService {
  bsModelRef: BsModalRef;

  constructor(private modalService: BsModalService) { }

  confirm(title = 'Confirmation', message = 'Are you sure ou want to do this?', btnOkText='Ok', btnCancelText = 'Cancel'): Observable<boolean> {  // Observable means we can subscribe to get true/false click event
    const config = {
      initialState: {
        title,
        message,
        btnOkText,
        btnCancelText
      }
    }
   // this.bsModelRef = this.modalService.show('confirm', config);
    this.bsModelRef = this.modalService.show(ConfirmDialogComponent, config);

    return new Observable<boolean>(this.getResult());
  }

private getResult() {                                                                                       // result here provides an observable so that we can use in our component above
  return (observer) => {
    const subscription = this.bsModelRef.onHidden.subscribe(() => {
      observer.next(this.bsModelRef.content.result);
      observer.complete();
    });

    return {
      unsubscribe() {
        subscription.unsubscribe();
      }
    }
  }
}
}
