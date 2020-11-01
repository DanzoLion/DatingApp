import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';                                                                 // ActivatedRoute
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { Member } from 'src/app/_models/member';
import { MembersService } from 'src/app/_services/members.service';


@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit {
  member: Member;
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];

  constructor(private memberService: MembersService, private route: ActivatedRoute) { }            // private route: ActivatedRoute brings in the selected user from Angular Router  // injected member service

  ngOnInit(): void {                                                               // -> we go to our API and retrieve send our member back and Angular has already loaded member-detail.component.html
    this.loadMember();

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

  loadMember() {
    this.memberService.getMember(this.route.snapshot.paramMap.get('username')).subscribe(member => {              // adjuster id to username in app-routing.module.ts
      this.member = member;
      this.galleryImages = this.getImages();            // added as once we get our member then we set our member here // we guarantee we have the photos before we load them via galleryImages
    })                  
  }

}
