import { Component, Input, OnInit, Self } from '@angular/core';
import { ControlValueAccessor, NgControl } from '@angular/forms';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';

@Component({
  selector: 'app-date-input',
  templateUrl: './date-input.component.html',
  styleUrls: ['./date-input.component.css']
})
export class DateInputComponent implements ControlValueAccessor {         // ControlValueAccessor implemented here   // implement interface DateInputComponent
  @Input() label: string;
  @Input() maxDate: Date;                                                                               // for max age user needs to be ie 18 yrs of age   // we then need to supply configuration to the datepicker
  bsConfig: Partial<BsDatepickerConfig>;                                                    // every property inside BsDatepickerConfigy Parial type is going to be optional ie partial config only need supplied

  constructor(@Self() public ngControl: NgControl) { 
  this.ngControl.valueAccessor = this;                                                  
  this.bsConfig = {                                                                                                 // we can then pass in some config options 
    containerClass: 'theme-red',
    dateInputFormat: 'DD MMMM YYYY'

  }
  }                                                                                                                           
  
  writeValue(obj: any): void {
    //throw new Error('Method not implemented.');
  }
  registerOnChange(fn: any): void {
    //throw new Error('Method not implemented.');
  }
  registerOnTouched(fn: any): void {
    //throw new Error('Method not implemented.');
  }
//  setDisabledState?(isDisabled: boolean): void {
   // throw new Error('Method not implemented.');
  }

  // ngOnInit(): void {
  //}

//}
