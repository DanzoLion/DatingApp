// import { HttpClient, HttpHeaders } from '@angular/common/http';
import { HttpClient } from '@angular/common/http';      // HttpHeaders removed after interceptor created
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';

/*const httpOptions = {                                                                                               // header added here as authentication is used by the endpoint users
    headers: new HttpHeaders({                                                                                // where we specify what headers we want to provide
    Authorization: 'Bearer ' + JSON.parse(localStorage.getItem('user'))?.token         // Authorization is the header: set to: 'Bearer ' then get JSON.parse() token from local storage and .token
    })
}*/                                                                                                                               // removed after our interceptor is fully working and complete

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getMembers()  {                                                                                           // returns observable of member and we specify type to tell it what we are receiving back from server <Member[]>
   //return this.http.get<Member[]>(this.baseUrl + 'users', httpOptions);      // enpoint is users // we also need to think about how we send up our authentication // users endpoint is protected by authentication // we need to add this as a header // httpOptions from defined header above
    return this.http.get<Member[]>(this.baseUrl + 'users');      // httpOptions removed
  }


  getMember(username: string){                                                                                         // we are explicit here to make sure we only return a string as parameter
 //   return this.http.get<Member>(this.baseUrl + 'users/' + username, httpOptions);
    return this.http.get<Member>(this.baseUrl + 'users/' + username); // httpOptions removed after we have created our interceptor: jwt.interceptor.ts
  }
}
