import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { ToastrService } from 'ngx-toastr';
import { BehaviorSubject } from 'rxjs';
import { take } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class PresenceService {
  hubUrl = environment.hubUrl;
  private hubConnection: HubConnection;
  private onlineUsersSource = new BehaviorSubject<string[]>([]);            // initialised to empty array   // behaviour subject as a subscriber 
  onlineUsers$ = this.onlineUsersSource.asObservable();                                             // $ dollar indicates observable

  constructor(private toastr: ToastrService, private router: Router) { }  

  createHubConnection(user: User){                                                          // this takes care of creating the hub connection
    this.hubConnection = new HubConnectionBuilder()
    .withUrl(this.hubUrl + 'presence', {
      accessTokenFactory: () => user.token
    })
    .withAutomaticReconnect()
    .build()

    this.hubConnection                                                                          // starts connection
    .start()
    .catch(error => console.log(error));

    this.hubConnection.on('UserIsOnline', username => {                   // listens for server events, ie user online or offline
      //this.toastr.info(username + ' has connected');                                                 // adjustment to improve notification
      this.onlineUsers$.pipe(take(1)).subscribe(usernames => {this.onlineUsersSource.next([...usernames, username])  // we are avoiding mutation here // we take in a list of usernames => 
      })
    })

    this.hubConnection.on('UserIsOffline', username => {
     // this.toastr.warning(username + ' has disconnected');                                     //  adjustment when a user logs off 
    this.onlineUsers$.pipe(take(1)).subscribe(usernames => {
      this.onlineUsersSource.next([...usernames.filter(x => x !== username)])
    })
    })

    this.hubConnection.on('GetOnlineUsers', (usernames: string[]) => {                // we create another listening event here to implement the observable
      this.onlineUsersSource.next(usernames);
    }) 

    this.hubConnection.on('NewMessageReceived', ({username, knownAs}) => {
      this.toastr.info(knownAs + ' has sent you a new message!')
      .onTap
      .pipe(take(1))
      .subscribe(() => this.router.navigateByUrl('/members/' + username + '?tab=3'))
    })
  }

  stopHubConnection() {
    this.hubConnection.stop().catch(error => console.log(error));
  }
}
