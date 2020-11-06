import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { Member } from 'src/app/_models/member';    // Member[]
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
  //members: Member[];                                                                // only type of members will be stored here
  members$: Observable<Member[]>;  // adjusted to an observable and is of type member[] array // adjusted here after making changes in member.service.ts // async pipe() to be implemented here .. member-list.component.html
  constructor(private memberService: MembersService) { }

  ngOnInit(): void {
    //this.loadMembers();                                                                 // loads-up our members
  this.members$ = this.memberService.getMembers();                   // gets members after adjusting to array observable // $ - indicates this is an observable
  }

  /*loadMembers() {                                                                                 // we don't need to manage error handling here as we have already done that in our error interceptor
    this.memberService.getMembers().subscribe(members =>{
      this.members = members; 
    })
  }*/                                                           // removed after making adjustments to observable and member[] type array 

}
