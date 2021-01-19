export interface Pagination {
    currentPage: number;
    itemsPerPage: number;
    totalItems: number;
    totalPages: number;
}

export class PaginatedResult<T>{    // we can use type parameter of <T>  so we can use PaginatedResult with any of our different types //  T effectively is an array of members <Member[]>
    result: T;                          // list of members stored in result
    pagination: Pagination;  // pagination will be stored here  .. ie pagination information
}