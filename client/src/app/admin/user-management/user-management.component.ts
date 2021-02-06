import { Component, OnInit } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { RolesModalComponent } from 'src/app/modals/roles-modal/roles-modal.component';
import { User } from 'src/app/_models/user';
import { AdminService } from '../admin.service';

@Component({
  selector: 'app-user-management',
  templateUrl: './user-management.component.html',
  styleUrls: ['./user-management.component.css']
})
export class UserManagementComponent implements OnInit {
  users: Partial<User[]>;
  bsModalRef: BsModalRef;

  constructor(private adminService: AdminService, private modalService: BsModalService) { }

  ngOnInit(): void {
    this.getUsersWithRoles();
  }

getUsersWithRoles() {
  this.adminService.getUsersWithRoles().subscribe(users => {
    this.users = users;
  })
}

openRolesModal(user: User) {
    // const initialState = {                                         // the initial ngx template config   // initial state
    //   list: [#                                                   // list
    //     'Open a modal with component',
    //     'Pass your data',
    //     'Do something else',
    //     '...'
    //   ],
    //   title: 'Modal with component'     // title
    // };
const config  = {
  class: 'modal-dialogue-centered',                          // shows modal in the centre of the screen
  initialState: {
    user, 
    roles: this.getRolesArray(user)
  }
}
    this.bsModalRef = this.modalService.show(RolesModalComponent, config);            // initialState removed and replaced with config
    this.bsModalRef.content.updateSelectedRoles.subscribe(values => {                               // the objective here is to send data to our modal
      const rolesToUpdate = {
        roles: [...values.filter(el => el.checked === true).map(el => el.name)]            // ... spread operator spreads the contents of the values -> then we filter -> el is element
      };

      if (rolesToUpdate) {
        this.adminService.updateUserRoles(user.username, rolesToUpdate.roles).subscribe(() => {
          user.roles = [...rolesToUpdate.roles]
        })
      }
    })
  //  this.bsModalRef = this.modalService.show(RolesModalComponent)
  }
  private getRolesArray(user){
    const roles = [];
    const userRoles = user.roles;
    const availableRoles: any[] = [
      {name: 'Admin', value: 'Admin'},
      {name: 'Moderator', value: 'Moderator'},
      {name: 'Member', value: 'Member'},
    ];

    availableRoles.forEach(role => {                        // we check for available roles by looping over availableRoles
      let isMatch = false;
      for (const userRole of userRoles) {
        if (role.name === userRole) {
          isMatch = true;
          role.checked = true;
          roles.push(role);
          break;
        }
      }
      if (!isMatch) {
        role.checked = false;
        roles.push(role);
      }
    })
    return roles;
  }

}

