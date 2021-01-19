using System.Linq;
using System;
using System.Collections.Generic; // List<>
using Microsoft.EntityFrameworkCore; // CountAsync()
using System.Threading.Tasks; // Task<>

namespace API.Helpers
{
    public class PagedList<T> : List<T>  // where <T> is generic and will take any type of entity // PagedList will be type of list and we will inherit from List<T> also generic ie list of users or list of members
    {   // we place our paging properties here // we initialise the constructor and replace the variables we don't need
        public PagedList(IEnumerable<T> items, int count, int pageNumber, int pageSize) // inside this constructor we pass in the items we get from our query .. IEnumerable<T>
        {
            CurrentPage = pageNumber;
            TotalPages = (int) Math.Ceiling(count / (double) pageSize); // ie total result count is 10, page size is 5, 2 is result, or if 4.5, max 5 is the result, or 2 pages
            PageSize = pageSize;
            TotalCount = count;
            AddRange(items);    // where we have access to the items inside PagedList
        }

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)  // we create a new instance of our PagedList class here, then we return it at the end
        {
            var count = await source.CountAsync();          // this does make a database call // if we want to work out total number of records returned
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(); // page number - 1, * page size, skip no records if 1, then take 5, will be 1, or 2 or ..
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}