import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { take } from 'rxjs/operators';
import { Member } from 'src/app/_models/member';    // Member[]
import { Pagination } from 'src/app/_models/pagination';
import { User } from 'src/app/_models/user';
import { UserParams } from 'src/app/_modules/userParams';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
  //members: Member[];                                                                // only type of members will be stored here
 // members$: Observable<Member[]>;                                     // adjusted to an observable and is of type member[] array // adjusted here after making changes in member.service.ts // async pipe() to be implemented here .. member-list.component.html
 members: Member[]; 
 pagination: Pagination;
//pageNumber = 1;
//pageSize = 5;
userParams: UserParams;
user: User;
genderList = [{value: 'male', display: 'Males'}, {value: 'female', display: 'Females'}];   // adding this as an array used for dropdown list for members gender


 constructor(private memberService: MembersService) {
   this.userParams = this.memberService.getUserParams();
 //constructor(private memberService: MembersService, private accountService: AccountService) {
  //  this.accountService.currentUser$.pipe(take(1)).subscribe(user => {
  //   this.user = user;
  //   this.userParams = new UserParams(user);
  //  })
  }

  ngOnInit(): void {
    //this.loadMembers();                                                                 // loads-up our members
  //this.members$ = this.memberService.getMembers();               // gets members after adjusting to array observable // $ - indicates this is an observable // removed and replaced with loadMembers()
    this.loadMembers();
}

  loadMembers() {
    this.memberService.setUserParams(this.userParams);
    this.memberService.getMembers(this.userParams).subscribe(response => {
      this.members = response.result;                                             // these components are now stored inside our members.Member[] component class properties
      this.pagination = response.pagination;
    })
  }

  /*loadMembers() {                                                                                 // we don't need to manage error handling here as we have already done that in our error interceptor
    this.memberService.getMembers().subscribe(members =>{
      this.members = members; 
    })
  }*/                                                           // removed after making adjustments to observable and member[] type array 

resetFilters() {                                                                  // resets filters of our gender types
  //this.userParams = new UserParams(this.user);
  this.userParams = this.memberService.resetUserParams();
  this.loadMembers();
}


pageChanged(event: any){
  this.userParams.pageNumber = event.page;
  this.memberService.setUserParams(this.userParams);
  this.loadMembers();
}

}
