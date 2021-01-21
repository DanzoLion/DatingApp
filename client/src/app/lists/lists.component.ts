import { Component, OnInit } from '@angular/core';
import { Member } from '../_models/member';
import { Pagination } from '../_models/pagination';
import { MembersService } from '../_services/members.service';

@Component({
  selector: 'app-lists',
  templateUrl: './lists.component.html',
  styleUrls: ['./lists.component.css']
})
export class ListsComponent implements OnInit {
  members: Partial<Member[]>;                                      // Partial defines an optional set of properties / 
  predicate = 'liked';
  pageNumber = 1;
  pageSize = 5;
  pagination: Pagination;

  constructor(private memberService: MembersService) { }   // we inject our memberService into our consturctor here

  ngOnInit(): void {
    this.loadLikes();
  }

  loadLikes() {
    //this.memberService.getLikes(this.predicate).subscribe(response => {
    this.memberService.getLikes(this.predicate, this.pageNumber, this.pageSize).subscribe(response => {
      this.members = response.result;
      this.pagination = response.pagination;
    })
  }

  pageChanged(event: any) {
    this.pageNumber = event.page;
    this.loadLikes();
  }
}

