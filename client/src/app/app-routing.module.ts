import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AdminPanelComponent } from './admin/admin-panel/admin-panel.component';
import { NotFoundComponent } from './errors/not-found/not-found.component';
import { ServerErrorComponent } from './errors/server-error/server-error.component';
import { TestErrorsComponent } from './errors/test-errors/test-errors.component';
import { HomeComponent } from './home/home.component';
import { ListsComponent } from './lists/lists.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MessagesComponent } from './messages/messages.component';
import { AdminGuard } from './_guards/admin.guard';
import { AuthGuard } from './_guards/auth.guard';
import { PreventUnsavedChangesGuard } from './_guards/prevent-unsaved-changes.guard';
import { MemberDetailedResolver } from './_resolvers/member-detailed.resolver';

const routes: Routes = [
  {path: "", component: HomeComponent},                                                                                         // empty string represents the path :4200; that is our home component

  {                                                            // these guards are accumulators to if we fail at any point the route will not be successful
    path:"", 
    runGuardsAndResolvers: "always",                                                                                          // decides when guards and resolvers will be run
    canActivate: [AuthGuard],                                                                                                       // protects children
    children: [                                                                                                                             // will be an array of our route // all of our children are now covered by our AuthGuard above
      //{path: "members", component: MemberListComponent}, // removed:  canActivate: [AuthGuard]},           // specify AuthGuard array [1 member] - after completing authGuard // route guard now active
      {path: "members", component: MemberListComponent,},           // specify AuthGuard array [1 member] - after completing authGuard // route guard now active
      {path: "members/:username", component: MemberDetailComponent, resolve: {member: MemberDetailedResolver}},    // changed id to username to decide which route the have gone to we access the username from route parameters
      {path: "member/edit", component: MemberEditComponent, canDeactivate: [PreventUnsavedChangesGuard],},           // added after generating ng g c member-edit component for routing // added new guard [PreventUnsavedChangesGuard] after creating prevent-unsaved-changes.guard.ts
    // {path: "members/:id", component: MemberDetailComponent},     // is the id of our member
      {path: "lists", component: ListsComponent,},                                                                         // is the id of our member
      {path: "messages", component: MessagesComponent,},                                              // all these items moved to children array/list
      {path: "admin", component: AdminPanelComponent, canActivate:[AdminGuard]},                                              // admin panel route
    ]
  },
     {path: "errors", component: TestErrorsComponent},                                                     // we add TestErrorsComponent after we have created the test-errors components.ts + html files from command line and coded these
     {path: "not-found", component: NotFoundComponent},                                                     // create this link after setting up not-found.component.html
     {path: "server-error", component: ServerErrorComponent},                                                     // create this link after setting up server-error.component.html
     {path: "**", component: NotFoundComponent, pathMatch: "full"},                                // ** wildcard route where user types in something that doesn't match anything in our config. //full redirects to home component based on member //changed: from HomeComponent -> NotFoundComponent
  
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
