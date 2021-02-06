import { Directive, Input, OnInit, TemplateRef, ViewContainerRef } from '@angular/core';
import { take } from 'rxjs/operators';
import { User } from '../_models/user';
import { AccountService } from '../_services/account.service';

@Directive({
  selector: '[appHasRole]'     // the selector we will specify will be *appHasRole as this is our structural directive
})
export class HasRoleDirective implements OnInit{              // implements OnInit interface
  @Input() appHasRole: string[];
  user: User;

  constructor(private viewContainerRef: ViewContainerRef, private templateRef: TemplateRef<any>, private accountService: AccountService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => {
      this.user = user;
    })
   }
  ngOnInit(): void {                                                                // the interface of OnInit above
  if (!this.user?.roles || this.user == null) {                             // clear view if there are no roles
    this.viewContainerRef.clear();
    return;
  }

  if (this.user?.roles.some(r => this.appHasRole.includes(r))) {                   // adds admin link below ¬ if they are in that role
    this.viewContainerRef.createEmbeddedView(this.templateRef);           // template ref is the <li> in nav.component.html class="nav-link" routerLink='/admin'
  } else {
    this.viewContainerRef.clear();                                                                // clears the ref if not in that role
  }
  }

}
