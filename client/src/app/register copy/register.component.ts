import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';       // decorator @Input() added here // also imports for Output()
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
                                                                            //removed as register(model) added in account.service.ts:  @Input() usersFromHomeComponent: any;          // called component explicitly as to where we derive it from 
  @Output() cancelRegister = new EventEmitter();
 // model: any= {};                                                       // removed as we are now implementing registerForm.value
  registerForm: FormGroup;                              // as we implement the form, we are aware we are dealing with registering users here  // sets up our reactive form
  maxDate: Date;                          // this property maxDate: has type of Date
 // validationErrors: string[];       // set to array of string   // where we get our error back from the interceptor // removed and replaced with validationErrors: string[] =[];
  validationErrors: string[] = [];    // we are checking for length of array in initialize component so we need to initialize array

  constructor(private accountService: AccountService, private toastr: ToastrService, private fb: FormBuilder, private router: Router) { }          // added private toastr: ToastrService // added fb: FormBuilder service
  // private router: Router added to re-direct our users to members page
  ngOnInit(): void {                                        // we can initialise our form from here here
  this.initializeForm();
  this.maxDate = new Date();                        // added when we configure our new datepicker with properties
  this.maxDate.setFullYear(this.maxDate.getFullYear() -18);         // restricts year to 18+
  
  }

// ** alterations to initilizeForm() below

  /*initializeForm() {
    //this.registerForm = new FormGroup({                         // the form group contains form controls     // altered with the injection of fb: FormBuilder
    this.registerForm = new FormGroup({                         // the form group contains form controls
      username: new FormControl('', Validators.required),                                                                                            // we add FormControl properties here to implement our form validation properties
      password: new FormControl('', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]),
      confirmPassword: new FormControl('', [Validators.required, this.matchValues('password')]),      // we need to pass in password as a validaor parameter to match values below
    })
  }*/

  initializeForm() {       // altered with the injection of fb: FormBuilder  // this now becomes a simplified version of our code // and is typically how we create a reactive form
      this.registerForm = this.fb.group({                         // the form group contains form controls
     // username: ['', Validators.required],                     // we add FormControl properties here to implement our form validation properties
      password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]],
      gender: ['male'],                     // we add FormControl properties here to implement our form validation properties   // we don't have a validator with radio button // user forced to make selection
      username: ['', Validators.required], 
      knownAs: ['', Validators.required], 
      dateOfBirth: ['', Validators.required], 
      city: ['', Validators.required], 
      country: ['', Validators.required], 
      confirmPassword: ['', [Validators.required, this.matchValues('password')]],       // NB order of properties here is not important
  })
  }


  matchValues(matchTo: string): ValidatorFn {           // here we are returning a string from our form fields so we matchTo: string
    return (control: AbstractControl) => {                        // AbstractControl returns from our FormControl
    return control?.value === control?.parent?.controls[matchTo].value ? null : {isMatching: true} // here we get access to the controller we are validating to // we attach this to our confirm password control
    }
    }


register() {
 // console.log(this.registerForm.value);                     // added and commented out belew .. this contains our values for our form control  // removed after form implentation complete
  this.accountService.register(this.registerForm.value).subscribe(response => {    // placed back our method to register user // replaced this.model with registerForm.Value
   // console.log(response);                                                             // first we remove console.log(response) then re-direct user   // here we will re-direct user to members page when they register
   // this.cancel();                        // removed
   this.router.navigateByUrl('/members');      // replaces this.console.log as we re-direct our users to members area  
  }, error => {                                           // we also need to take care of errors from the servers side also // mismatch of data between form and register form
  //  console.log(error);         // removed as we are now implmenting error handling from our interceptor and implementing validationError
  this.validationErrors = error;
 //   this.toastr.error(error.error);        // removed as part of member re-direct       // error logging via toasr added here
 }
  )                                           //console.log(this.model);
}

cancel() {
  this.cancelRegister.emit(false);                                        //console.log("cancelled");
}
}
