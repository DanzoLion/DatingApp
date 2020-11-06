import { Injectable } from '@angular/core';
import { CanDeactivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { MemberEditComponent } from '../members/member-edit/member-edit.component';

@Injectable({
  providedIn: 'root'
})
export class PreventUnsavedChangesGuard implements CanDeactivate<unknown> {     // we activate CanDeactivate interface here
  canDeactivate(component: MemberEditComponent,) :  boolean {                                 // gives access to our memberEdit Form
    if (component.editForm.dirty)   {                                                                                    // we have access to component here // check to see if dirty ie altered
      return confirm('Are you sure you wish to continue? Unsaved changes will be lost .. ') // provides option of yes or no, if true will activate component 
    }
    return true;                                                                                                                      // if no, user remains on the form and no changes made

   
    /*Observable<boolean | UrlTree> | Promise<boolean | UrlTree>*/ // removed from boolean |  UrlTree above 
      /*currentRoute: ActivatedRouteSnapshot,     // removed as not req.
    currentState: RouterStateSnapshot, // removed as not req.
    nextState?: RouterStateSnapshot)*/ // removed as not required
   
   
  }
  
}
