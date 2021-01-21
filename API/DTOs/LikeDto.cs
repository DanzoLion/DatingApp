namespace API.DTOs
{
    public class LikeDto                                   // the idea is to display the users as cards and return this DTO data for the cards
    {
        public int Id { get; set; }                                        
        public int Age { get; set; }
        public string Username { get; set; }
        public string KnownAs { get; set; }
        public string PhotoUrl { get; set; }
        public string City { get; set; }
        
    }
}