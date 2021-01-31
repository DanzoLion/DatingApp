namespace API.Helpers
{
    public class MessageParams : PaginationParams
    {
     public string Username { get; set; }   
     public string Container { get; set; } = "Unread";               // our default is here, the unread messages to the user
    }
}