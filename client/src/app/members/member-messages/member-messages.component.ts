import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Message } from 'src/app/_models/message';
import { MembersService } from 'src/app/_services/members.service';
import { MessageService } from 'src/app/_services/message.service';

@Component({
  selector: 'app-member-messages',
  templateUrl: './member-messages.component.html',
  styleUrls: ['./member-messages.component.css']
})
export class MemberMessagesComponent implements OnInit {
 // @Input() username: string;                                                                                                  // the username of the member name we just clicked on
@ViewChild('messageForm') messageForm: NgForm;
@Input() messages: Message[];
@Input() username: string;
messageContent: string;

 // constructor( private messageService: MessageService) { }
  constructor(private messageService: MessageService ) { }

  ngOnInit(): void {
//    this.loadMessages();
  }

  sendMessage(){
    this.messageService.sendMessage(this.username, this.messageContent).subscribe(message =>{
      this.messages.push(message);
      this.messageForm.reset();
    })
  }
}
