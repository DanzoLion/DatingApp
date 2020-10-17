import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-test-errors',
  templateUrl: './test-errors.component.html',
  styleUrls: ['./test-errors.component.css']
})
export class TestErrorsComponent implements OnInit {
  baseUrl = 'https://localhost:5001/api/';
  validationErrors: string[] = [];                                                                 // the type of this property will be string array // and initialise to an empty array

  constructor(private http: HttpClient) { }                                        // http service so we can test our error responses from API

  ngOnInit(): void {
  }

  get404Error()  {
    this.http.get(this.baseUrl + 'buggy/not-found').subscribe(response => {                                 // hits our buggy controller and returns 'buggy/not-found; subscribe then check response
    console.log(response);                                                                                                              // for each of these errors what we want to do is see what we get back from the client first of all
    }, error => {
      console.log(error);
    })                       
  }

  get400Error()  {
    this.http.get(this.baseUrl + 'buggy/bad-request').subscribe(response => {                                 // hits our buggy controller and returns 'buggy/not-found; subscribe then check response
    console.log(response);                                                                                                              // for each of these errors what we want to do is see what we get back from the client first of all
    }, error => {
      console.log(error);
    })                       
  }

  get500Error()  {
    this.http.get(this.baseUrl + 'buggy/server-error').subscribe(response => {                                 // hits our buggy controller and returns 'buggy/not-found; subscribe then check response
    console.log(response);                                                                                                              // for each of these errors what we want to do is see what we get back from the client first of all
    }, error => {
      console.log(error);
    })                       
  }

  get401Error()  {
    this.http.get(this.baseUrl + 'buggy/auth').subscribe(response => {                                 // hits our buggy controller and returns 'buggy/not-found; subscribe then check response
    console.log(response);                                                                                                              // for each of these errors what we want to do is see what we get back from the client first of all
    }, error => {
      console.log(error);
    })                       
  }

  get400ValidationError()  {
    this.http.post(this.baseUrl + 'account/register', {}).subscribe(response => {                                 // hits our buggy controller and redirects to account/register; 
    console.log(response);                                                                                                              // for each of these errors what we want to do is see what we get back from the client first of all
    }, error => {
      console.log(error);
      this.validationErrors = error;                                                                              // as we know we are throwing back an array of errors here
    })                       
  }

}
