namespace API.DTOs
{
    public class UserDto                                                // this is the object we will return when the user logs in or registers
    {
        public string Username { get; set; }

        public string Token { get; set; }

        public string PhotoUrl { get; set; }                        // added after setting main photo in API // this adds main photo image to the nav bar
    }
}