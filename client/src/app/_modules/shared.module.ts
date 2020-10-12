import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ToastrModule } from 'ngx-toastr';



@NgModule({
  declarations: [],
  imports: [
    CommonModule,                                               // all angular modules need this CommonModule and this is automatically imported
    BsDropdownModule.forRoot(),                         // moved to here from app.module.ts                          // added from ngx bootstrap: components //.forRoot() means it has services and components it needs to initialise along with root module
    ToastrModule.forRoot({                                    // moved to here from app.module.ts                           // added toasr and imported with parameters here
      positionClass: "toast-bottom-right"
    })
  ],

  exports: [
    BsDropdownModule,
    ToastrModule
  ]


})
export class SharedModule { }
