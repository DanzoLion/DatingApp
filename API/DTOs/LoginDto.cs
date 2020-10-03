namespace API.DTOs              // created this Dto to manage the Login() method username+password in AccountController.cs and HttpPost endpoint
{
    public class LoginDto
    {
        public string Username { get; set; }

        public string Password { get; set; }
    }
}