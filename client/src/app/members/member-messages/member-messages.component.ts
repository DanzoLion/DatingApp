import { ChangeDetectionStrategy, Component, Input, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Message } from 'src/app/_models/message';
import { MembersService } from 'src/app/_services/members.service';
import { MessageService } from 'src/app/_services/message.service';

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,                               // added to fix error when loading member-message component, for window adjustment / scrolling
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
loading = false;

 // constructor( private messageService: MessageService) { }                            // we create an async method so we can subscribe to the messages
  constructor(public messageService: MessageService ) { }

  ngOnInit(): void {
//    this.loadMessages();
  }

  sendMessage(){
    this.loading = true;
    this.messageService.sendMessage(this.username, this.messageContent).then(() =>{
    //this.messageService.sendMessage(this.username, this.messageContent).subscribe(message =>{   // we don't subscribe because this is now a promise not an objservable
     // this.messages.push(message);      // removed after message refactor // we instead recieve this from SignalR hub
      this.messageForm.reset();
    }).finally(() => this.loading = false);
  }
}
