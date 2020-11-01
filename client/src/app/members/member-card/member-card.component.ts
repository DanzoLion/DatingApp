import { Component, Input, OnInit } from '@angular/core';
import { Member } from 'src/app/_models/member';


@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css'],
})
export class MemberCardComponent implements OnInit {
  @Input() member: Member;                                                                  // input property is member and its type we call Member;   // @input() is the property

  constructor() { }

  ngOnInit(): void {
  }
}
