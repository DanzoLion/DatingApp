import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from "@angular/router";
import { Observable } from "rxjs";                                                                                                                          // implemented when we import interface Resolve and method
import { Member } from "../_models/member";
import { MembersService } from "../_services/members.service";

@Injectable({                      // injectable operator
    providedIn: 'root'                                               
})
export class MemberDetailedResolver implements Resolve<Member>{         // resolve is instantiated in the same way as services

    constructor(private memberService: MembersService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<Member>  {
        return this.memberService.getMember(route.paramMap.get('username'));         // we don't need to subscribe as the router takes care of this for us
    }       // implements Resolve interface from Angular     

}        