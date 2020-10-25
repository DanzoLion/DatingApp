using System;
using System.Collections.Generic;                                                                               // <IEnumerable> // allows simple iteration over a collection type <List> can be used but has more features than simple <IEnumerable>
using System.Linq;                                                                                                       // ToList() -> we are getting our data from database and converting into list
using System.Threading.Tasks;                                                                       // because we use asynchronous threading with Task<ActionResults>
using API.Data;                                                                                             // required because we are using DataContext
using API.DTOs;                                                                                           // MemberDto
using API.Entities;                                                                                                     // AppUser
using API.Interfaces;                                                                                   // IUserRepository
using AutoMapper;                                                                                       // IMapper
using Microsoft.AspNetCore.Authorization;                                                   // [Authorize]
using Microsoft.AspNetCore.Mvc;                                                                     // Mvc: Model View Controller // view comes from client // how we use .NET to serve HTML pages // c is what we use as we are going to use Angular
using Microsoft.EntityFrameworkCore;                                                        // ToListAsync() - for asynchronous threading

namespace API.Controllers {
                                                                                                                        // [ApiController]    // removed as we now inherit from BaseApiController attributes    // attributes specified here
      [Authorize]                                                                                                           // all methods inside controller are now protected with [Authorize]                                                                                                  
                                                                                                                         // [Route ("api/[controller]")]  // removed as we now inherit from BaseApiController attributes      // to get to controller, users specify api/controller 
    public class UsersController : BaseApiController                                            // first derive from ControllerBase   // dependency injection will be used to get data from database here // modified: to BaseApiController
    {
        //private readonly DataContext _context;                                                  // _context allows us to access DataBase  // removed with userRepository implemented
        private readonly IUserRepository _userRepository;                                // inserted after UsersController updated
        private readonly IMapper _mapper;

        // public UsersController (DataContext context)                                  // quick fix used to generate constructor for our controller // parameters added to constructors // quick fix used to initilise field from parameter // updated
        public UsersController(IUserRepository userRepository, IMapper mapper)                   // inject IUserRepository Interface after we created it and call it userRepository
        {
        //    _context = context;                                                 // quick fix used to initialise field from parameter // inside our class we now have access to datavase via DataContext // removed with userRepository implemented 
            _userRepository = userRepository;                                                       // created after updated IUserRepository 
            _mapper = mapper;                                                   // added with new property created IMapper
        }                                                                                                                           // two endpoints will be added to get a single user from database, and to get all users from database

        [HttpGet]                                                                                                               // getting data 
 //       [AllowAnonymous]                                                                                            // for GetUsers()       // for testing/comparing two different req. we get back in PostMan          
      //  public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()         // AppUser changed  // return the result from our get request, return result back as a list     //IEnumerable for returning collection list of type AppUser // method GetUsers()
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()   
        {                                                                                                                                   // async added for asynchronous threading // then wrap ActionResult<> into a task for asynchronous threading
            //return await _context.Users.ToListAsync();        // variable created here to store users // specify we want to get our users from list into database // await for asynchronous threading // removed with usersRepository implementation 
            //var users = await _userRepository.GetUserAsync();               // first deposit await into users variable    // changed to single line wrapper below
          //  return Ok(await _userRepository.GetUserAsync());                                              // return users;   // we return users from endpoint // change .ToList() to ToListAsync() - for asynchronous threading    // changed to return Ok(users);
          // var users = await _userRepository.GetUserAsync();          **************** changed upon new implementation of new AutoMapper implementation ********************
         //  var usersToReturn = _mapper.Map<IEnumerable<MemberDto>>(users);          // we have access to _mapper once we create the property and inherit the class   // this is what we are mapping to in this case       // removed    

         var users = await _userRepository.GetMembersAsync();
        return Ok(users);
          //  return Ok(usersToReturn);                                         // cut here  await _userRepository.GetUserAsync() and placed int var users  // changed after new AutoMapper implementation

        }                                                                                                                                     // await results of task until appropriate results are returned
      //  [Authorize]                                                                     // Authorize attribute added here to authenticate our endpoint // our GetUsers(int id) endpoint is now protected  // [Authorize] moved to top of class
      //  [HttpGet("{id}")]                                                               // getting data of individual user and via their Id // id as root parameter // ie api/users/2  -where int is the id of user we are fetching
       [HttpGet("{username}")]
              // public async Task<ActionResult<AppUser>> GetUsers(int id)       // return the result from our get request, return result back as a list     //remove: IEnumerable for returning collection list of type AppUser not returning a collection // method GetUsers(int id) -parameter req. int id
        // public async Task<ActionResult<AppUser>> GetUsers(string username)     // GetUsers() via username is the new method we use to get individual users


         public async Task<ActionResult<MemberDto>> GetUsers(string username)   // changed from <AppUser> to <MemberDto> // we niw return our MemberDto
        {                                                                                                       // return user; removed as we don't need to specify a variable  // we return users from endpoint // added async Task<> & await .FindAsync
           // return await _context.Users.FindAsync(id);                            // variable created here to store users // specify we want to get our users from list into database // Find method finds entity with given primary key from database // don't need a variable as we dont use it
          //  return await _userRepository.GetUserByUsernameAsync(username);          // we now get the user via username instead of ID   // re-written to handle Dto

        //var user = await _userRepository.GetUserByUsernameAsync(username);      // AutoMember no takes care of automapping between our AppUser and MemberDto // changed when implementing new _mapper in UserRepository.cs
       return await _userRepository.GetMemberAsync(username);      // AutoMember no takes care of automapping between our AppUser and MemberDto
       // return _mapper.Map<MemberDto>(user);                    // returns memberDto // also returns user         // removed after adding .GetMemberAsync(username) as we are implementing new _mapper implementation 

        }

    }
}