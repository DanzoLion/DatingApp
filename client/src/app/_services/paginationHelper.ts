import { HttpClient, HttpParams } from "@angular/common/http";
import { map } from "rxjs/operators";
import { PaginatedResult } from "../_models/pagination";

export function getPaginatedResult<T>(url, params, http: HttpClient) {                                                                                         // created this new method ie refactor, generate new Extracted to method in class 'MemberService'
const paginatedResult: PaginatedResult<T> = new PaginatedResult<T>();
return http.get<T>(url, { observe: 'response', params }).pipe(
  map(response => {
    paginatedResult.result = response.body; // members array will be contained inside response.body
    if (response.headers.get('Pagination') !== null) { // we then check our pagination headers here
      paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
    }
    return paginatedResult;
  }));
}

export function getPaginationHeaders(pageNumber: number, pageSize: number) {
  let params = new HttpParams();
  // if (page !== null && itemsPerPage !== null){   // can be removed as we already initialise page size and page number above, this is removed
  params = params.append('pageNumber', pageNumber.toString());
  params = params.append('pageSize', pageSize.toString());

  return params;
}