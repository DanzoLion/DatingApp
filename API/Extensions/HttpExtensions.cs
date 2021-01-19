using API.Helpers; // PaginationHeader .. we created this earlier
using Microsoft.AspNetCore.Http;  // HttpResponse
using System.Text.Json;

namespace API.Extensions
{
    public static class HttpExtensions              // static as it can be accessed anywhere as an extension
    {                                                                   // we will create our method to return header in here
        public static void AddPaginationHeader(this HttpResponse response, int currentPage, int itemsPerPage,  int totalItems, int totalPages)  // our PaginationHeader variables
        {
            var paginationHeader = new PaginationHeader(currentPage, itemsPerPage, totalItems, totalPages);                  // this is our header

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase                                       // added this option to convert our header form title case to camel case
            };

            response.Headers.Add("Pagination", JsonSerializer.Serialize(paginationHeader, options));     
            // only req. here is to give our header a sensible name // we then need to searialise the header as it contains a reponse, key
            // options added here after working through pagination code .. var options added to convert case
            
            // we need to add a cause header to make "Pagination" header available // we do this below
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination"); // these two variables need to match exactly, exat string matches
            // we then create a new class to receive the pagination parameters from the user
        }
    }
}