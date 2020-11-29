using System;
using System.ComponentModel.DataAnnotations;        // we import for [Required]

namespace API.DTOs
{
    public class RegisterDto
    { 
        [Required]                                                              // specifies username is req. + password    // error to produce: One or more validation errors occurred // returns array of objects returned that are in error
        public string Username { get; set; }
        [Required] public string KnownAs {get; set;}                // below properties are added once we implement our datepicker
        [Required] public string Gender {get; set;}
        [Required] public DateTime DateOfBirth {get; set;}
        [Required] public string City {get; set;}
        [Required] public string Country {get; set;}


        [Required]
        [StringLength(8, MinimumLength = 4)]                // additional validator added here so that we see responses when we hit this particular method // after createing BuggyController.cs 
         public string Password { get; set;}                        //   public string Password { get; set; }                    
    }
}