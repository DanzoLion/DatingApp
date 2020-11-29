import { Component, Input, OnInit } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';
import { take } from 'rxjs/operators';
import { Member } from 'src/app/_models/member';
import { Photo } from 'src/app/_models/photo';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css']
})
export class PhotoEditorComponent implements OnInit {
  @Input() member: Member;                                                        // we add this as our input property member from Member;
  uploader: FileUploader;                                                               // these properties are added after we complete our FileUploadModlue imports with shared.module.ts
  hasBaseDropzoneOver = false;
  baseUrl = environment.apiUrl;
  user: User;                                                                                   // user property added as we need to authToken and retrieve user on initializeUploader

  constructor(private accountService: AccountService, private memberService: MembersService) {             // account service injected here as we configure with account controller and retrieve user below
      // we also inject memberService: MembersService to work with setMainPhoto(photo) method below:
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user);                                                            // we then need to get our user and extract from the observable
   }           

  ngOnInit(): void {
    this.initializeUploader();                                                    // after we configure the uploader we can then initialise it here
  }

  fileOverBase(e: any){  // method to set our dropzone inside a template         e: = event of any
  this.hasBaseDropzoneOver = e;
  }

  setMainPhoto(photo: Photo) { // here we add our method to set main photo, we need to inject our parameter class above
    this.memberService.setMainPhoto(photo.id).subscribe(() => {
      this.user.photoUrl = photo.url;
      this.accountService.setCurrentUser(this.user);          // updates our current user observable and updates our current user in local storage // gets photo our of our browser and display on our navbar
      this.member.photoUrl = photo.url;
      this.member.photos.forEach(p => {                       // goes through each photo and switches main photo that is true to false, and current photo to true
        if (p.isMain) p.isMain = false;                               // removes current main photo
        if (p.id === photo.id) p.isMain = true;               // sets/replaces main photo with current photo
      })
    })

  }

deletePhoto(photoId: number) {
  this.memberService.deletePhoto(photoId).subscribe(() => {                 //.subscribe() is blank because we don't return anything here
this.member.photos = this.member.photos.filter(x => x.id !== photoId);  // returns an array of all photos that are not equal to photo id we are passing into photoId: number .. filters out all our photos 
  })  // we don't allow user to delete main photo .. // our interceptor is taking care of the error handling for us
}


initializeUploader() {  
  this.uploader = new FileUploader({                                          // configuration properties added here
    url: this.baseUrl + 'users/add-photo',                                      // users/add-photo is our endpoint
    authToken: 'Bearer '  + this.user.token,                                   // here we get our user from our account controller
    isHTML5: true,
    allowedFileType: ['image'],
    removeAfterUpload: true,
    autoUpload: false,
    maxFileSize: 10 * 1024 *1024
  });                                                               // completes the uploader configuration

this.uploader.onAfterAddingFile = (file) => {
  file.withCredentials = false;
}

 this.uploader.onSuccessItem = (item, response, status, headers) => {   // we then specify what we need to do after the uploader has completed 
if (response) {                                                                                             // we check fro a response here 
  const photo: Photo = JSON.parse(response);
  this.member.photos.push(photo);                                                         // then push our photo's into photo array
  if (photo.isMain) {                                                                   // we add this check to see if photo is selected, if photo then select for all user, member, accountService ie everywhere
    this.user.photoUrl = photo.url;
    this.member.photoUrl = photo.url;
    this.accountService.setCurrentUser(this.user);
  }
} 

}

}
}
