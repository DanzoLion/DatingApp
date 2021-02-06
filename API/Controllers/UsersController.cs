using System.Security.Claims;         // claim types
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
using Microsoft.AspNetCore.Http;                                                             // IFormFile
using API.Extensions;
using API.Helpers;

namespace API.Controllers {
                                                                                                                                                                                                   // [ApiController]    // removed as we now inherit from BaseApiController attributes    // attributes specified here
      [Authorize]                                                                                                                                                                          // all methods inside controller are now protected with [Authorize]                                                                                                  
                                                                                                                                                                                                     // [Route ("api/[controller]")]  // removed as we now inherit from BaseApiController attributes      // to get to controller, users specify api/controller 
    public class UsersController : BaseApiController                                                                                                                          // first derive from ControllerBase   // dependency injection will be used to get data from database here // modified: to BaseApiController
    {
                                                                                                                                                                                                         //private readonly DataContext _context;                                                  // _context allows us to access DataBase  // removed with userRepository implemented
        private readonly IUserRepository _userRepository;                                                                                                                        // inserted after UsersController updated
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;


        // public UsersController (DataContext context)                                  // quick fix used to generate constructor for our controller // parameters added to constructors // quick fix used to initilise field from parameter // updated
        public UsersController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService)                                         // inject IUserRepository Interface after we created it and call it userRepository // added IPhotoService as we are adding our Photo service below
        {
                                                                                                                                                                                                 //    _context = context;                                                 // quick fix used to initialise field from parameter // inside our class we now have access to datavase via DataContext // removed with userRepository implemented 
            _userRepository = userRepository;                                                                                                                                          // created after updated IUserRepository 
            _mapper = mapper;                                                                                                                                                               // added with new property created IMapper
            _photoService = photoService;
            // added as we need this variable for our PhotoService implementation
        }                                                                                                                                                                                                      // two endpoints will be added to get a single user from database, and to get all users from database


      //  [Authorize(Roles = "Admin")]          // only Admin can get a full list of users here
        [HttpGet]                                                                                                                                                                                     // getting data 
                                                                                                                                                                                                            //       [AllowAnonymous]                                                                                            // for GetUsers()       // for testing/comparing two different req. we get back in PostMan          
                                                                                                                                                                                                                  //  public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()         // AppUser changed  // return the result from our get request, return result back as a list     //IEnumerable for returning collection list of type AppUser // method GetUsers()
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers([FromQuery]UserParams userParams)   // added UserParams when createing PagedList + UserRepository config.   // FromQuery needs to be specified as it's a Query String Param API controller needs this attribute
        {                                                                                                                                                                                                // async added for asynchronous threading // then wrap ActionResult<> into a task for asynchronous threading
                                                                                                                                                                                                        //return await _context.Users.ToListAsync();        // variable created here to store users // specify we want to get our users from list into database // await for asynchronous threading // removed with usersRepository implementation 
                                                                                                                                                                                                        //var users = await _userRepository.GetUserAsync();               // first deposit await into users variable    // changed to single line wrapper below
                                                                                                                                                                                                      //  return Ok(await _userRepository.GetUserAsync());                                              // return users;   // we return users from endpoint // change .ToList() to ToListAsync() - for asynchronous threading    // changed to return Ok(users);
                                                                                                                                                                                                      // var users = await _userRepository.GetUserAsync();          **************** changed upon new implementation of new AutoMapper implementation ********************
                                                                                                                                                                                                    //  var usersToReturn = _mapper.Map<IEnumerable<MemberDto>>(users);          // we have access to _mapper once we create the property and inherit the class   // this is what we are mapping to in this case       // removed    
        var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
         userParams.CurrentUsername = user.UserName;            // get username of user; then add a check for their gender to set default gender
        
        if (string.IsNullOrEmpty(userParams.Gender))
          userParams.Gender = user.Gender == "male" ? "female" : "male";            // checks if user is male or femal, if female set to male 

         var users = await _userRepository.GetMembersAsync(userParams);                                                             // we are then able to pass userParms here once it's implemented above
                                                                                                                                                                               // users variable then becomes type of PagedList<MemberDto> // pagination infor inside here
        Response.AddPaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);   // we have access to our responside inside our controllers // we always have access to http response data
        
        return Ok(users);
                                                                                                                                                  //  return Ok(usersToReturn);                                         // cut here  await _userRepository.GetUserAsync() and placed int var users  // changed after new AutoMapper implementation

        }                                                                                                                                     // await results of task until appropriate results are returned
                                                                                                                                            //  [Authorize]                                                                     // Authorize attribute added here to authenticate our endpoint // our GetUsers(int id) endpoint is now protected  // [Authorize] moved to top of class
                                                                                                                                            //  [HttpGet("{id}")]                                                               // getting data of individual user and via their Id // id as root parameter // ie api/users/2  -where int is the id of user we are fetching
     //  [Authorize(Roles = "Member")]                        // Member can get an individual user
       [HttpGet("{username}", Name = "GetUser")]                                                                // we add Name = GetUser as a root name for CreatedAtRoute()
                                                                                                                                                                                      // public async Task<ActionResult<AppUser>> GetUsers(int id)       // return the result from our get request, return result back as a list     //remove: IEnumerable for returning collection list of type AppUser not returning a collection // method GetUsers(int id) -parameter req. int id
                                                                                                                                                                                // public async Task<ActionResult<AppUser>> GetUsers(string username)     // GetUsers() via username is the new method we use to get individual users


         public async Task<ActionResult<MemberDto>> GetUsers(string username)                                            // changed from <AppUser> to <MemberDto> // we niw return our MemberDto
        {                                                                                                                                                                   // return user; removed as we don't need to specify a variable  // we return users from endpoint // added async Task<> & await .FindAsync
                                                                                                                                                                          // return await _context.Users.FindAsync(id);                            // variable created here to store users // specify we want to get our users from list into database // Find method finds entity with given primary key from database // don't need a variable as we dont use it
                                                                                                                                                                          //  return await _userRepository.GetUserByUsernameAsync(username);          // we now get the user via username instead of ID   // re-written to handle Dto

                                                                                                                                                                                   //var user = await _userRepository.GetUserByUsernameAsync(username);      // AutoMember no takes care of automapping between our AppUser and MemberDto // changed when implementing new _mapper in UserRepository.cs
       return await _userRepository.GetMemberAsync(username);                                                                                 // AutoMember no takes care of automapping between our AppUser and MemberDto
                                                                                                                                                                                         // return _mapper.Map<MemberDto>(user);                    // returns memberDto // also returns user         // removed after adding .GetMemberAsync(username) as we are implementing new _mapper implementation 
        }

                                                                                                                                                                                  // method added here .. what is relevant is the method / parameters / route used // in this case HttpPut is the only Put method and it is unique and all we need
      [HttpPut]                                                                                                                                                                 // HttpPut is the resource/method we use to alter/make changes/resource on a server
      public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)                                                     // our method // returns action result // don't need to send data back as client has all data we have for entity we are creating here
      {
                                                                                                                                                                                                  // what do we want to do when we update a Dto?  We need the user and username
                                                                                                                                                                                                  // we are getting the username from what we are authenticating against, ie against the token
                                                                                                                                                                                                  //FindFirst() is the claims principle of the user .. inside a controller we have access to it here // contains information about their identity
                                                                                                                                                                                                  // we are matching the name identifier, the identifier given to the name that matches the name/identifier in their token
                                                                                                                                                                                                  //var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // should provide token of the user form username // this is the user we are updating // added ClaimsPrincipleExtensions.cs to clean this up
                                                                                                                                                                                                  //var username = User.GetUsername(); // should provide token of the user form username // this is the user we are updating // turned into one liner below, method inserted as variable below
           var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());                                                   // once we have user we use the user repository and user by user async
           
           _mapper.Map(memberUpdateDto, user);                                                                                                            // we map the user to Dto here
           _userRepository.Update(user);                                                                                                                         // here the user object is flagged as being updated by entity framework // guarantees we don't get an error or exception

           if (await _userRepository.SaveAllAsync()) return NoContent();                                                                           // no content sent back here and saves if successful

           return BadRequest("Failed to update user");                                                                                                  // if not _userRepository .. return BadRequest
      }

    [HttpPost("add-photo")]                                                                                                                                              // we add this as a new resource after we created IPhotoService.cs and PhotoService.cs // root parameter "add-photo"
    public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)                                                              // our client to get a few values and return a few variables here depending on the logic we are going to add ie Id or MainPhoto // allows user to upload file and is simple without additonal info
    {
        var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());                                       // user object also converted to a one-liner 
        var result = await _photoService.AddPhotoAsync(file);                                                                                    // add file to _photoService

        if (result.Error != null) return BadRequest(result.Error.Message);                                                                            // we check if there is an error from the upload ie the upload will work or it won't // if not null // result will be from cloudinary
        
        var photo = new Photo                                                                                                                                      // create a new photo
        {
          Url = result.SecureUrl.AbsoluteUri,
          PublicId = result.PublicId                                  
        };

        if (user.Photos.Count == 0)                                                                                                                                     // Here we need to check if the user has any photo's at this stage // current // if 0 == true it is the first photo being uploaded
          {
            photo.IsMain = true;
          }

      user.Photos.Add(photo);     
      if (await _userRepository.SaveAllAsync())                                                                                                                    // if true save all photo's
      {
         //return _mapper.Map<PhotoDto>(photo);                                                                                                             // moved to here
         return CreatedAtRoute("GetUser", new {username = user.UserName}, _mapper.Map<PhotoDto>(photo));         // Created .. all the options that return 201 Created result / GetUser: photo
      }
    //  return _mapper.Map<PhotoDto>(photo);                                                                                                                // map photo via Dto

    return BadRequest("Problem Adding Photo ..");                                                        // if fail

    }

  [HttpPut("set-main-photo/{photoId}")]                                                             // root: "set-main-photo/ then we pass in photoId as root parameter   // for updating main photo in API

  public async Task<ActionResult> SetMainPhoto(int photoId)                       // when updating a task we don't need to pass the object back / return the object
  {                                                                                                               // server has signed token, user has sent the token, and we can trust this is correct, the user then authenticates to the method below
    // GetUserByUsernameAsync() has an eager loading property for this method and we have access to photo's inside here, 
    var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());  // here we get our user by their username // when we get user from token as in this case we say the user is who they say they are, ie no validation necessary here
    // FirstOrDefault() is not async as we already have user in memory at this point, ie extracted from _userRepository,  x => x.Id looks for the photo we get from the parameter photoId
    var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);  // next we get the photo that matches this specific id

    if (photo.IsMain)     // this checks whether the user is trying to set a photo that already is main to main and we will prevent the user from doing this on the client
    return BadRequest("This is already your Main Photo Hombre .. "); // if user gets past this then we need to get hold of the current photo and make it false, and use update photo to true ..

    var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);                  // checks for current photo, make it false and update with new photo ..
    if (currentMain != null) currentMain.IsMain = false;                                 // here we to check if NULL   .. ie turn main photo off  
    photo.IsMain = true;                                                                                // turn main photo on

    if (await _userRepository.SaveAllAsync()) return NoContent();                   // next we need to save our changes back to the _userRepository //  .. return NoContent() as we don't need to send anything back
    return BadRequest("Failed to set main photo");                              // in the event that something else went wrong, return "Failed to set main photo"
  }

  [HttpDelete("delete-photo/{photoId}")]                             // our resource delete method that will delete our photos // we specify root name delete-photo and id
  public async Task<ActionResult> DeletePhoto(int photoId)                              // ActionResult added here as we won't be sending anything back to the client after we delete it
{
  var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());          // first we need to get our user object  // we retrieve the user by username // then below identify phot we are interested in only
  var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);                 // once photo identified, we need to do a few checks below
  if (photo == null) return NotFound();                     // NotFound if we can't find the user in the photo collection
  if (photo.IsMain) return BadRequest("You cannot delete Main Photo!");                   // so that we cannot delete our main photo
  if (photo.PublicId != null)                     // we want to check for a public id -- we only want to delete those photo's from cloudinary that have a public id
  {
    var result = await _photoService.DeletePhotoAsync(photo.PublicId);            // checks the public id here and we store this in a variable
    if (result.Error != null) return BadRequest(result.Error.Message);                  // if not null, stop execution and return bad request, with error message  // otherwise we assume sucess below
      }
  user.Photos.Remove(photo);          // we then remove user photos and pass in photo entity  // adds a tracking flag and updates user at this point  // photo is a realted member on our entity
  if (await _userRepository.SaveAllAsync()) return Ok();        // if complete we return simple Ok // if not successful saving this return bad request below

  return BadRequest("Fail to delete your photo man :( ");
  }
}
}