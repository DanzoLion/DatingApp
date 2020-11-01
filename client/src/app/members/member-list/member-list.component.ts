import { Component, OnInit } from '@angular/core';
import { Member } from 'src/app/_models/member';    // Member[]
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
  members: Member[];                                                                // only type of members will be stored here

  constructor(private memberService: MembersService) { }

  ngOnInit(): void {
    this.loadMembers();                                                                 // loads-up our members
  }

  loadMembers() {                                                                                 // we don't need to manage error handling here as we have already done that in our error interceptor
    this.memberService.getMembers().subscribe(members =>{
      this.members = members; 
    })
  }

}
