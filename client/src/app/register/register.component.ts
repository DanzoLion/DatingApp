import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';       // decorator @Input() added here // also imports for Output()
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
                                                                            //removed as register(model) added in account.service.ts:  @Input() usersFromHomeComponent: any;          // called component explicitly as to where we derive it from 
  @Output() cancelRegister = new EventEmitter();

  model: any= {};

  constructor(private accountService: AccountService, private toastr: ToastrService) { }          // added private toastr: ToastrService

  ngOnInit(): void {
  }

register() {
  this.accountService.register(this.model).subscribe(response => {
    console.log(response);
    this.cancel();
  }, error => {
    console.log(error);
    this.toastr.error(error.error);                                                                                               // error logging via toasr added here
  }
  )                                           //console.log(this.model);
}

cancel() {
  this.cancelRegister.emit(false);                                        //console.log("cancelled");
}


}
