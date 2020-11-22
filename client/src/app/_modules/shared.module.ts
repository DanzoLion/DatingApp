import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ToastrModule } from 'ngx-toastr';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { NgxGalleryModule } from '@kolkov/ngx-gallery';
import { FileUploadModule } from 'ng2-file-upload';


@NgModule({
  declarations: [],
  imports: [
    CommonModule,                                               // all angular modules need this CommonModule and this is automatically imported
    BsDropdownModule.forRoot(),                         // moved to here from app.module.ts                          // added from ngx bootstrap: components //.forRoot() means it has services and components it needs to initialise along with root module
    ToastrModule.forRoot({                                    // moved to here from app.module.ts                           // added toasr and imported with parameters here
      positionClass: "toast-bottom-right"
    }),
    TabsModule.forRoot(),                                        // because we are using a shared module we need to export it also as we won't be able to use it otherwise
    NgxGalleryModule,                                            // imported as we are setting up our photo gallery, we have installed: npm install@kolkov/ngx-gallery // also need to export  
    FileUploadModule,                                             // we import this after we install the file uploader component ng2
  ],

  exports: [
    BsDropdownModule,
    ToastrModule,
    TabsModule,
    NgxGalleryModule,
    FileUploadModule,
  ]


})
export class SharedModule { }
