namespace API.Helpers
{
    public class UserParams
    {
        // first we set the max. number of items we return as a request:
        private const int MaxPageSize = 50;             // if over 50 and not default then 50 will be returned as the max.
        public int PageNumber { get; set; } = 1;        // this is taken from the user
        private int _pageSize = 10;                           // this is our default page size

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value; // get the value, if value greater set it to value, if not take max value and set it to supplied max value ie 50
        }

        public string CurrentUsername {get; set;}
        public string Gender { get; set; }
        public int MinAge { get; set; } = 18;                               // added for age filtering
        public int MaxAge { get; set; } = 150;                          // added for age filtering
        public string OrderBy {get; set;} = "lastActive";              // added to provide the user options for newest members and last active members
    }
}