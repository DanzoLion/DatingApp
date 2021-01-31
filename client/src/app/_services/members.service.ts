// import { HttpClient, HttpHeaders } from '@angular/common/http';
import { HttpClient, HttpParams } from '@angular/common/http';      // HttpHeaders removed after interceptor created
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';
import { of, pipe } from 'rxjs';
import { map, take } from 'rxjs/operators';
import { PaginatedResult } from '../_models/pagination';
import { UserParams } from '../_modules/userParams';
import { AccountService } from './account.service';
import { User } from '../_models/user';
import { getPaginatedResult, getPaginationHeaders } from './paginationHelper';

/*const httpOptions = {                                                                                               // header added here as authentication is used by the endpoint users
    headers: new HttpHeaders({                                                                                // where we specify what headers we want to provide
    Authorization: 'Bearer ' + JSON.parse(localStorage.getItem('user'))?.token         // Authorization is the header: set to: 'Bearer ' then get JSON.parse() token from local storage and .token
    })
}*/                                                                                                                               // removed after our interceptor is fully working and complete

@Injectable({
  providedIn: 'root'
})
export class MembersService {   // we alter our members list component to determine how we get our members list -> member-list.component.ts
  baseUrl = environment.apiUrl;
  members: Member[] = [];                          // this is for storing our server status and we create this after setting up our loading display feature // add array as empty array
 // paginatedResult: PaginatedResult<Member[]> = new PaginatedResult<Member[]>();   // our new pagination array implementation // results will be stored here
 memberCache = new Map();     // used to map our string output as keys to store as our cache
 user: User;
 userParams: UserParams;
 
  constructor(private http: HttpClient, private accountService: AccountService) { 
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => {
      this.user = user;
      this.userParams = new UserParams(user);
    })
    }

    getUserParams() {
      return this.userParams;
    }

    setUserParams(params: UserParams) {
      this.userParams = params;
    }

    resetUserParams() {
      this.userParams = new UserParams(this.user);
      return this.userParams;
    }


  //getMembers(page?: number, itemsPerPage?: number)  {  // returns observable of member and we specify type to tell it what we are receiving back from server <Member[]> // parameters added
  getMembers(userParams: UserParams)  {  // returns observable of member and we specify type to tell it what we are receiving back from server <Member[]> // parameters added
   // if (this.getMembers.length > 0) return of(this.members)       // created to check that our array define above is zero .. then return a member as an observable // client observes data // of returns 'of' an observable // the we set members after get them from API do this via .pipe() // turned off for pagination implementation
   //return this.http.get<Member[]>(this.baseUrl + 'users', httpOptions);      // enpoint is users // we also need to think about how we send up our authentication // users endpoint is protected by authentication // we need to add this as a header // httpOptions from defined header above
   // let params = new HttpParams();
  // console.log(Object.values(userParams).join('-'));    // our test print to console to test our userParams functionality
  var response = this.memberCache.get(Object.values(userParams).join('-'));
  if(response) {
    return of(response);
  }
   
    let params = getPaginationHeaders(userParams.pageNumber, userParams.pageSize);
    params = params.append('minAge', userParams.minAge.toString());
    params = params.append('maxAge', userParams.maxAge.toString());
    params = params.append('gender', userParams.gender);
    params = params.append('orderBy', userParams.orderBy);
  /*if (page !== null && itemsPerPage !== null){
     params = params.append('pageNumber', page.toString());
     params = params.append('pageSize', itemsPerPage.toString());
   }*/
      return getPaginatedResult<Member[]>(this.baseUrl + 'users', params, this.http)
      .pipe(map(response => {
        this.memberCache.set(Object.values(userParams).join('-'), response);
        return response;
      }))


      //return this.newMethod(params)
      // removed below, with pagination implementation: this was our caching code
      /*map(members => {                                                          // map object returns the values as an observable
        this.members = members;
        return members;                                                           // returned members here are returned as an observable
      })*/
     // )
    }


  getMember(username: string){                                                                                         // we are explicit here to make sure we only return a string as parameter
 //   return this.http.get<Member>(this.baseUrl + 'users/' + username, httpOptions);
    // const member = this.members.find(x => x.userName === username);      // here we attempt to get the member we have inside our service ie where user may refresh their page and we don't have anything inside the page 
    // // === is JavaScript equality  // we find the member of the same username we are passing in as a parameter
    // if(member !== undefined) return of(member);         // if not undefined .. return of(member) // if we don't have member then moves onto API call below:
//console.log(this.memberCache);
    const member = [...this.memberCache.values()]
    .reduce((arr, elem) => arr.concat(elem.result), [])
    .find((member: Member) => member.userName === username);

    if (member){
      return of(member);
    }
    //console.log(member);
    //console.log(member);
    return this.http.get<Member>(this.baseUrl + 'users/' + username); // httpOptions removed after we have created our interceptor: jwt.interceptor.ts
  }

  updateMember(member: Member){  // method takes in member of type: Member
    // we then need to configure our updateMember .. so the old data is not seen if we are calling from the above array // we get our member from service now 
    // if we are updating a member and we don't do anything with it here the user will see the old data, it needs to be updated here
    return this.http.put(this.baseUrl + 'users', member).pipe( // member passed in as object to updateMember method // pipe introduced to manage updated member data
      map(() => {       // here we get member from service
        const index = this.members.indexOf(member);   // gets the member that matches the member of our index
        this.members[index] = member;         // specify member then set this to our index
      })
      )
    }

    setMainPhoto(photoId: number) {
      return this.http.put(this.baseUrl + 'users/set-main-photo/' + photoId, {});     // this adds a method for our user to set their main photo
    }


    deletePhoto(photoId: number) {
      return this.http.delete(this.baseUrl + 'users/delete-photo/' + photoId);
    }

    addLike(username: string) {
      return this.http.post(this.baseUrl + 'likes/' + username, { });     // post requires empty { } body
    }

    getLikes(predicate: string, pageNumber, pageSize) {
      let params = getPaginationHeaders(pageNumber, pageSize);
      params = params.append('predicate', predicate)
      //return this.http.get<Partial<Member[]>>(this.baseUrl + 'likes?predicate=' + predicate);    // will be either liked or liked by via predicate // updated: <Partial<Member[]>>
      return getPaginatedResult<Partial<Member[]>>(this.baseUrl + 'likes', params, this.http);
    }

    // --------------> moved down to here:


}
