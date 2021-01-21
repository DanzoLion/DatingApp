namespace API.Helpers
{
    public class UserParams : PaginationParams
    {
        // removed pagination code to new class PaginationParams.cs

        public string CurrentUsername {get; set;}
        public string Gender { get; set; }
        public int MinAge { get; set; } = 18;                               // added for age filtering
        public int MaxAge { get; set; } = 150;                          // added for age filtering
        public string OrderBy {get; set;} = "lastActive";              // added to provide the user options for newest members and last active members
    }
}