import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { BehaviorSubject } from 'rxjs';
import { take } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Group } from '../_models/group';
import { Message } from '../_models/message';
import { User } from '../_models/user';
import { getPaginatedResult, getPaginationHeaders } from './paginationHelper';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  baseUrl = environment.apiUrl;
  hubUrl = environment.hubUrl;
  private hubConnection: HubConnection;   
  private messageThreadSource = new BehaviorSubject<Message[]>([]);                 // this deals with messages we receive from the hub, initialised as an empty array        
  messageThread$ = this.messageThreadSource.asObservable();                      // defined observable that deals with messages received from hub $ denotes observable            

  constructor(private http: HttpClient) { }

  createHubConnection(user: User, otherUsername: string) { 
    this.hubConnection = new HubConnectionBuilder()
    .withUrl(this.hubUrl + 'message?user=' + otherUsername, {
      accessTokenFactory: () => user.token
    })
    .withAutomaticReconnect()
    .build()
    this.hubConnection.start().catch(error => console.log(error));

    this.hubConnection.on('ReceiveMessageThread', messages => {        // gets the message thread when we join the group
      this.messageThreadSource.next(messages);
    })

    this.hubConnection.on('NewMessage', message => {           // the idea is to update our messages with a new message array, and not mutate the current array
      this.messageThread$.pipe(take(1)).subscribe(messages => {      // updates the message as read
        this.messageThreadSource.next([...messages, message])   // spread operator that creates a new array to populate our message subject // mutation does not take place, rather replacement does
      })
    })

    this.hubConnection.on('UpdatedGroup', (group: Group) => {
      if (group.connections.some(x => x.username === otherUsername)) {
        this.messageThread$.pipe(take(1)).subscribe(messages => {
          messages.forEach(message => {
            if (!message.dateRead) {
              message.dateRead = new Date(Date.now())
           }
        })
        this.messageThreadSource.next([...messages]);                  // ...messages creates a new array and shouldn't interfere with Angular change tracking
      })
    }
  })
  }

  stopHubConnection() {
    if (this.hubConnection) {                                                   // stops hub only if it is in existence, to handle connection issues if users disconnect
      this.hubConnection.stop();                                            // acts as a safety conditional for connection issues
    }
   //  this.hubConnection.stop();
  }

  getMessages(pageNumber, pageSize, container) {
    let params = getPaginationHeaders(pageNumber, pageSize);
    params = params.append('Container', container);
    return getPaginatedResult<Message[]>(this.baseUrl + 'messages', params, this.http);
  }

getMessageThread(username: string) {
  return this.http.get<Message[]>(this.baseUrl + 'messages/thread/' + username);
}

async sendMessage(username: string, content: string){           // async implemented here after refactor  // guarantees we return a promise from this method
 // return this.http.post<Message>(this.baseUrl + 'messages', {recipientUsername: username, content})   // API call replaced with hub feature
  return this.hubConnection.invoke('SendMessage', {recipientUsername: username, content})     // we use the hub to send the message instead of API call // this returns a promise instead of observable
  .catch(error => console.log(error));    // this error is created if there is no longer an http request available
}

deleteMessage(id: number) {
  return this.http.delete(this.baseUrl + 'messages/' + id);
}
}
