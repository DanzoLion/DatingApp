import { Component, Input, OnInit, Self } from '@angular/core';
import { ControlValueAccessor, NgControl } from '@angular/forms';

@Component({
  selector: 'app-text-input',
  templateUrl: './text-input.component.html',
  styleUrls: ['./text-input.component.css']
})
export class TextInputComponent implements ControlValueAccessor {   // implemented from TextInputComponent    // we inject the control of the constructor() into component ControlValueAccessor
  @Input() label: string;                                                         // string with type 'text' label
  @Input() type= 'text';                                                          // input properties we'll be passing into our methods // text will be the default property

                                                                                              // these methods are required methods that are mandatory for this component
  constructor(@Self() public ngControl: NgControl ) {         // the functions are written by the ControlValueAccessor .. so we don't declare any method code // @self ensure local injection
  this.ngControl.valueAccessor = this;                          // this allows access to our control when we register/use it inside our controlValueAccessor
  }             
  writeValue(obj: any): void {                                                  // these methods are used to pass data through, and we don't need to enter data here
  //  throw new Error('Method not implemented.');
  }
  registerOnChange(fn: any): void {
  //  throw new Error('Method not implemented.');
  }
  registerOnTouched(fn: any): void {
 //   throw new Error('Method not implemented.');
  }
  
  //setDisabledState?(isDisabled: boolean): void {          // this fourth method is not required
  //  throw new Error('Method not implemented.');
  }

  //ngOnInit(): void { // removed as we do not implement the ngOnInit interface
 // }
//}
