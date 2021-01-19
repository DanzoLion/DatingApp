namespace API.DTOs
{
    public class UserDto                                                // this is the object we will return when the user logs in or registers
    {
        public string Username { get; set; }

        public string Token { get; set; }

        public string PhotoUrl { get; set; }                        // added after setting main photo in API // this adds main photo image to the nav bar

        public string KnownAs { get; set; }                      // added after completing datepicker we register our user knownAs
    
        public string Gender { get; set; }                          // we send back gender, this is for default gender value // saves completing an API call when the user is logged in this information will always be available
    }
}