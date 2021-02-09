import { query } from '@angular/animations';
import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';                                                                 // ActivatedRoute
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { TabDirective, TabsetComponent } from 'ngx-bootstrap/tabs';
import { take } from 'rxjs/operators';
import { Member } from 'src/app/_models/member';
import { Message } from 'src/app/_models/message';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';
import { MessageService } from 'src/app/_services/message.service';
import { PresenceService } from 'src/app/_services/presence.service';


@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit, OnDestroy{                    // OnDestroy stops connection if user moves away from hub connection completely
  @ViewChild('memberTabs', {static: true}) memberTabs: TabsetComponent;                          // accessed member-detail.component.html        <tabset class="member-tabset" #memberTabs>
  member: Member;
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];
  activeTab: TabDirective;
  messages: Message[] = [];
  user: User;                                                         // this property to deal with receiving messages

  //constructor(private memberService: MembersService, private route: ActivatedRoute, private messageService: MessageService) { }            // private route: ActivatedRoute brings in the selected user from Angular Router  // injected member service
  constructor(public presence: PresenceService, private route: ActivatedRoute, private messageService: MessageService, private accountService: AccountService, private router: Router) {
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user);   // this gives access to our current user
    this.router.routeReuseStrategy.shouldReuseRoute = () => false;   // to control the routes of our users so the message service can update in a more controlled way
   }            // private route: ActivatedRoute brings in the selected user from Angular Router  // injected member service // AccountService to deal with messaging
  


  ngOnInit(): void {                                                               // -> we go to our API and retrieve send our member back and Angular has already loaded member-detail.component.html
    //this.loadMember();
    this.route.data.subscribe(data => {
      this.member = data.member;
    })

    this.route.queryParams.subscribe(params => {
      params.tab ? this.selectTab(params.tab) : this.selectTab(0);
    })

    this.galleryOptions = [
      {
        width: '500px',                                                                                   // inside this array we provide an object of our different options
        height: '500px',
        imagePercent: 100,
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Slide,
        preview: false
        }
    ]
                                                                            // moved gallery images to here
    this.galleryImages = this.getImages();            // added as once we get our member then we set our member here // we guarantee we have the photos before we load them via galleryImages

    //this.galleryImages = this.getImages();                            // this will initialise our gallery // removed: this.galleryImages = this.getImages()

  }

  getImages(): NgxGalleryImage[] {          // here we add a method to get our images outside of our member: Member object
    const imageUrls = [];
    for (const photo of this.member.photos) {
      imageUrls.push({                                // we push our image photo's inside our image urls we created earlier // { } indicates this is an object 
        small: photo?.url,                            // ? optional chaining parameter used if null is returned this will keep our program alive and not cause error message
        medium: photo?.url,
        big: photo?.url
      })
    }
    return imageUrls;
  }

  // loadMember() {
  //   this.memberService.getMember(this.route.snapshot.paramMap.get('username')).subscribe(member => {              // adjuster id to username in app-routing.module.ts
  //     this.member = member;
  //    // this.galleryImages = this.getImages();            // added as once we get our member then we set our member here // we guarantee we have the photos before we load them via galleryImages
  //   })                  
  // }

  loadMessages() {
    this.messageService.getMessageThread(this.member.userName).subscribe(messages => {
      this.messages=messages;
    })
  }

  selectTab(tabId: number){
    this.memberTabs.tabs[tabId].active = true;
  }

  onTabActivated(data: TabDirective){
    this.activeTab = data;
    if(this.activeTab.heading === 'Messages' && this.messages.length === 0){      // implements re-use of messages instead of re-loading messages
     // this.loadMessages();
      this.messageService.createHubConnection(this.user, this.member.userName);
    } else {                                                                                                                  // disconnects if user moves away from the users tab
      this.messageService.stopHubConnection();
    }
  }

  ngOnDestroy(): void {                                                          // OnDestroy stops connection if user moves away from hub connection completely
    this.messageService.stopHubConnection();                          // replaced default error message with stopHubConnection()    // after implementing this we need to go to message.service.ts to adapt for disconneciton issues
  }
}
