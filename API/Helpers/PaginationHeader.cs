namespace API.Helpers   // will contain similar properties that we have in PagedList .. we create this after creating PagedList.cs
{
    public class PaginationHeader               // this is all the information we want to send back to the client .. in our header
    {
        public PaginationHeader(int currentPage, int itemsPerPage, int totalItems, int totalPages)              // we then generate constructor from PaginationHeader // and new instance of variables
        {
            CurrentPage = currentPage;
            ItemsPerPage = itemsPerPage;
            TotalItems = totalItems;
            TotalPages = totalPages;
        }

        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
    }
}