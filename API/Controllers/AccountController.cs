using System.Linq;
using System.Text;                                                             // imported for encoding
using System.Security.Cryptography;
using System.Threading.Tasks;                                       // importing Task
using API.Data;                                                             // as we are importing DataContext  
using API.Entities;                                                         // importing for AppUser
using Microsoft.AspNetCore.Mvc;                                 // impoting for ActionResult
using API.DTOs;                                                              // importing the Dto class we created RegisterDto.cs
using Microsoft.EntityFrameworkCore;                        // using AnyAsync()
using API.Interfaces;                               //ITokenService

namespace API.Controllers {
    public class AccountController : BaseApiController {                                                                                 // API class inherits from BaseApiController // automatically binds any parameters it finds for our method below Register()
        private readonly DataContext _context;                                                                                               // field / variable
        private readonly ITokenService _tokenService;

        public AccountController (DataContext context, ITokenService tokenService) {                                                                                 // constructor   // inject our new token service into account controller
            _context = context;                                                                                                                              // initiliser
            _tokenService = tokenService;                                                                                                           // we have the option to use _ or this
        }

        [HttpPost("register")]                                                                                                                              // method required to create new user // http POST // adds new resource to API endpoint
                                                                                                                                                                   // Register() method will need parameters to receive from POST request
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)                              // ActionResult to return element from out controller task/method  // element return as action result // AppUser specifies item we are returning method.name = Register()
        {                                                                                                                                                        // using accesses dispose method within HMACSHA512() to dispose cleanly // uses iDisposable interface 

            if(await UserExists(registerDto.Username))  return BadRequest("Username is taken");              //keep it simple using a single line here // BadRequest is accessed via ActionResult http requests // checks before user signs out
            using var hmac = new HMACSHA512();                                                                                    // HMACSHA512 is the hashing algorithm we need to create a password hash // using statement ensure when class closes it has finished correctly

            var user = new AppUser
            {                                                                                                                                                    // NB: we want our username to be unique so we create the method below UserExists
                UserName = registerDto.Username.ToLower(),                                                                       // NB: cant convert NULL into byte array on the next iteration // will create DTO class to solve this // specified .ToLower() username stored to lowercase
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),      // returns bytearray and returns bytearray as parameter // need to get the hash of our password using encoding as parameter
                PasswordSalt = hmac.Key                                                                                                         // .Key used as this is randomly generated from HMACSHA512()
            };

            _context.Users.Add(user);                                                                                                            // tells EF we want to add user to collections/table via Add() // only tracks via EF
            await _context.SaveChangesAsync();                                                                                            // this is where we call our database and save user into users table
            
            return new UserDto                                                                                                                 // this completes our register method  // implementing new UserDto instead of AppUser
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)                                                                         // token is our token service, and we pass user as a parameter
            };
        }

        [HttpPost("login")]                                                                                                                                      //login endpoint created here // sends values in body of request using Httppost // method called login
        public async Task<ActionResult<UserDto>>Login(LoginDto loginDto)                                                //return an action result and return AppUser to reflect user successfully logged in // Dto contains username+password but we want a unique Dto
        {                                                                                                                                                                // we then add the login for what we need inside this method // HttpPost Endpoint
            var user = await _context.Users
            .Include(p => p.Photos)                                                                                                     // photo included so our users photo is not empty  // eagerly loaded photos here
            .SingleOrDefaultAsync(x => x.UserName == loginDto.Username);          // we retrieve a user from our database via specific retrieve method // either found a user or not
            if (user == null)   return Unauthorized("Invalid Username");                                                            // if not found, return Unauthorised .. "Invalid Username"
             using var hmac = new HMACSHA512(user.PasswordSalt);                                                              // checks our HMACSHA // if found // does reverse calculation of registration // default constructor uses random Key
                                                                                                                                                                // key passed into HMACSHA512() is the password salt
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));         // computes the hash contained in loginDto // both hashes worked out should be identical at this point

            for (int i = 0; i < computedHash.Length; i++)                                                                           // need to loop over each element of byte array[]
            {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");       // if the loop fails and there is no match this will be implemented // companing every element
            }
            return new UserDto                                                                                                                 // this completes our register method  // implementing new UserDto instead of AppUser
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user),                                                                         // token is our token service, and we pass user as a parameter              // we then need to specify config properties for user
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url                                               // this property is addd in UserDto.cs .. and contains our Main Photo // exception will occur if no photo to work with, not NULL
            };                                                                                                                                              // hits here once the password has successfully been calculated and returns user object                   
        }                                                                                                                                        

        private async Task<bool> UserExists(string username)                                                        // test if there is the same entry in our databse, if is entry we return true, if not we return false
        {
            return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());                 // here we pass in expression to see if any username matches username in database table
        }
    }
}