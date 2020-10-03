using System.ComponentModel.DataAnnotations;        // we import for [Required]

namespace API.DTOs
{
    public class RegisterDto
    { 
        [Required]                                                              // specifies username is req. + password    // error to produce: One or more validation errors occurred // returns array of objects returned that are in error
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }                    
    }
}