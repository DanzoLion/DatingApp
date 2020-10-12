import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { ListsComponent } from './lists/lists.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MessagesComponent } from './messages/messages.component';
import { AuthGuard } from './_guards/auth.guard';

const routes: Routes = [
  {path: "", component: HomeComponent},                                                                                         // empty string represents the path :4200; that is our home component

  {
    path:"", 
    runGuardsAndResolvers: "always",                                                                                          // decides when guards and resolvers will be run
    canActivate: [AuthGuard],                                                                                                       // protects children
    children: [                                                                                                                             // will be an array of our route // all of our children are now covered by our AuthGuard above
      {path: "members", component: MemberListComponent, canActivate: [AuthGuard]},           // specify AuthGuard array [1 member] - after completing authGuard // route guard now active
      {path: "members/:id", component: MemberDetailComponent},                                              // is the id of our member
      {path: "lists", component: ListsComponent},                                                                         // is the id of our member
      {path: "messages", component: MessagesComponent},                                              // all these items moved to children array/list
    ]
  },
  {path: "**", component: HomeComponent, pathMatch: "full"},                                              // ** wildcard route where user types in something that doesn't match anything in our config. //full redirects to home component based on member
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
