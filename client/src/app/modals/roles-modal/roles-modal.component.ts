import { Component, EventEmitter, Input, OnInit } from '@angular/core';
//import { EventEmitter } from 'events';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { User } from 'src/app/_models/user';

@Component({
  selector: 'app-roles-modal',
  templateUrl: './roles-modal.component.html',
  styleUrls: ['./roles-modal.component.css']
})
export class RolesModalComponent implements OnInit {
  // title: string;                                                                          // properties are passed into BsModalRef and go into modal and disaply the properties we have
  // list: any[] = [];
  // closeBtnName: string;
@Input() updateSelectedRoles = new EventEmitter();      // input required to recieve something from our component // we emit roles from a particular component here
user: User;
roles: any[];                                                                            


  constructor(public bsModalRef: BsModalRef) { }                // this goes into our modal and displays the properties we can provide

  ngOnInit(): void {
  }

updateRoles() {
  this.updateSelectedRoles.emit(this.roles);
  this.bsModalRef.hide();                                                          // turns the modal off here
}
}
