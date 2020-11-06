import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { Member } from 'src/app/_models/member';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {
  @ViewChild('editForm') editForm: NgForm;                       // we are accessing our template form #editForm="ngForm" inside our member-edit.component.html form
  member: Member;                                                                           // to bring in user from account service, one is member the other is user
  user: User;                                                                                         // we are populating our current user from our accountService below
  @HostListener('window:beforeunload', ['$event']) unloadNotifacation($event: any){  // allows us to access our browser event 'window:beforeunload'  and provide event ['$event']                                     
  if (this.editForm.dirty) {                                                                    // conditional check to see if changes made outside the browser
    $event.returnValue = true;
  } // @HostListener allows us to do something before the browser is closed 
}
  constructor(private accountService: AccountService, private memberService: MembersService, private toastr: ToastrService) {  // current user is an observable we need to extract the user first // toast added for updateMember()
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user);
  }

  ngOnInit(): void {
    this.loadMember();
    }

    loadMember() {
      this.memberService.getMember(this.user.username).subscribe(member => {
        this.member = member;
    })
  }

  updateMember() {                  // we create this method once we have adjusted our member form and created <#editForm="ngForm"> referencing member-edit.component.html
    // console.log(this.member); // removed after adding UpdateMember()  method inside member.service.ts
    // this.memberService.updateMember .. below replaces existing code 
    // this.toastr.success('Profile Updated Successfully');
    // this.editForm.reset(this.member);  // added to reset our form once we are able to access it using @ViewChild / this.member keeps and preserves the values in our form
  
  this.memberService.updateMember(this.member).subscribe(() => {
    this.toastr.success('Profile Updated Successfully');
    this.editForm.reset(this.member);  // added to reset our form once we are able to access it using @ViewChild / this.member keeps and preserves the values in our form
  })

  }

}
