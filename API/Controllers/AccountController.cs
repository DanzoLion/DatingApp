using System.Linq;
using System.Text;                                                             // imported for encoding
using System.Security.Cryptography;
using System.Threading.Tasks;                                       // importing Task
using API.Data;                                                             // as we are importing DataContext  
using API.Entities;                                                         // importing for AppUser
using Microsoft.AspNetCore.Mvc;                                 // impoting for ActionResult
using API.DTOs;                                                              // importing the Dto class we created RegisterDto.cs
using Microsoft.EntityFrameworkCore;                        // using AnyAsync()
using API.Interfaces;                                                         //ITokenService
using AutoMapper;                                                         // IMapper
using Microsoft.AspNetCore.Identity;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {                                                                                 // API class inherits from BaseApiController // automatically binds any parameters it finds for our method below Register()
        //private readonly DataContext _context;  //removed with implementation of identity manager                  // field / variable
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

       //  public AccountController(DataContext context, ITokenService tokenService, IMapper mapper)
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService, IMapper mapper)
        {     // constructor   // inject our new token service into account controller // IMapper to map form properties
            _mapper = mapper;
      //      _context = context;      // removed                                                                                          // initiliser
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;                                                                                                           // we have the option to use _ or this
        }

        [HttpPost("register")]                                                                                                                              // method required to create new user // http POST // adds new resource to API endpoint
                                                                                                                                                            // Register() method will need parameters to receive from POST request
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)                              // ActionResult to return element from out controller task/method  // element return as action result // AppUser specifies item we are returning method.name = Register()
        {                                                                                                                                                        // using accesses dispose method within HMACSHA512() to dispose cleanly // uses iDisposable interface 

            if (await UserExists(registerDto.Username)) return BadRequest("Username is taken");              //keep it simple using a single line here // BadRequest is accessed via ActionResult http requests // checks before user signs out
             
             var user = _mapper.Map<AppUser>(registerDto);                                                              // to AppUser from registerDto
           //     using var hmac = new HMACSHA512();    // removed with identity management        // HMACSHA512 is the hashing algorithm we need to create a password hash // using statement ensure when class closes it has finished correctly
                     //   var user = new AppUser                                                // removed with implmentation of IMapper
         //   {                                                                                                                                                    // NB: we want our username to be unique so we create the method below UserExists
                user. UserName = registerDto.Username.ToLower();                                                                       // NB: cant convert NULL into byte array on the next iteration // will create DTO class to solve this // specified .ToLower() username stored to lowercase
                // user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));      // returns bytearray and returns bytearray as parameter // need to get the hash of our password using encoding as parameter
                // user.PasswordSalt = hmac.Key;             //removed with Identity Management                                      // .Key used as this is randomly generated from HMACSHA512()
                             // };
            // _context.Users.Add(user);              // all removed with identity manager                                 // tells EF we want to add user to collections/table via Add() // only tracks via EF
            // await _context.SaveChangesAsync();                                                                                            // this is where we call our database and save user into users table
            var result = await _userManager.CreateAsync(user, registerDto.Password);           // both creates our user and saves changes into database   // implemented with identity manager     

            if (!result.Succeeded) return BadRequest(result.Errors);                    // checks for errors

             var roleResult = await _userManager.AddToRoleAsync(user, "Member");                             // here we place any registered user into the members role
            if(!roleResult.Succeeded) return BadRequest(result.Errors);                    // checks to see if roleResult is successful

            return new UserDto                                                                                                                 // this completes our register method  // implementing new UserDto instead of AppUser
            {
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user),     // token is our token service, and we pass user as a parameter  // updated to await with asynchronous method implemented in TokenService.cs
                KnownAs = user.KnownAs,                                                                                             // we add a KnownAs property to UserDto.cs    
                Gender = user.Gender                                                                        
            };
        }

        [HttpPost("login")]                                                                                                                                      //login endpoint created here // sends values in body of request using Httppost // method called login
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)                                                //return an action result and return AppUser to reflect user successfully logged in // Dto contains username+password but we want a unique Dto
        {                                                                                                                                                                // we then add the login for what we need inside this method // HttpPost Endpoint
           // var user = await _context.Users   // removed with identity manager
            var user = await _userManager.Users
            .Include(p => p.Photos)                                                                                                     // photo included so our users photo is not empty  // eagerly loaded photos here
            .SingleOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());          // we retrieve a user from our database via specific retrieve method // either found a user or not
            if (user == null) return Unauthorized("Invalid Username");                                                            // if not found, return Unauthorised .. "Invalid Username"
       // using var hmac = new HMACSHA512(user.PasswordSalt);    // removed with identity management    // checks our HMACSHA // if found // does reverse calculation of registration // default constructor uses random Key
            // key passed into HMACSHA512() is the password salt
            // var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));         // computes the hash contained in loginDto // both hashes worked out should be identical at this point
            // for (int i = 0; i < computedHash.Length; i++)         // removed with identity management                // need to loop over each element of byte array[]
            // {
            //     if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");       // if the loop fails and there is no match this will be implemented // companing every element
            // }
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);    // here we implement the signInManager

            if (!result.Succeeded) return Unauthorized();    // checks to see if password correct 


            return new UserDto                                                                                                                 // this completes our register method  // implementing new UserDto instead of AppUser
            {
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user),      // token is our token service, and we pass user as a parameter              // we then need to specify config properties for user // await updated
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,                                               // this property is addd in UserDto.cs .. and contains our Main Photo // exception will occur if no photo to work with, not NULL
                KnownAs = user.KnownAs,                                                                                             // KnownAs property added to UserDto.cs // this makes use of NavBar
                Gender = user.Gender
            };                                                                                                                                              // hits here once the password has successfully been calculated and returns user object                   
        }
        private async Task<bool> UserExists(string username)                                                        // test if there is the same entry in our databse, if is entry we return true, if not we return false
        {
            //return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());                 // here we pass in expression to see if any username matches username in database table
            return await _userManager.Users.AnyAsync(x => x.UserName == username.ToLower());           // removed with identity manager
        }
    }
}