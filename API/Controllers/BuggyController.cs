using API.Data;                                                           // DataContext
using API.Entities;                                                     // AppUser
using Microsoft.AspNetCore.Authorization;               // Authorize
using Microsoft.AspNetCore.Mvc;                             // ActionResult

namespace API.Controllers {
    public class BuggyController : BaseApiController {
        private readonly DataContext _context;                   // variable name // field name
        public BuggyController (DataContext context) // we inject our data context inside here
        {
            _context = context;                                         // field initialised from parameter
        }
        
        [Authorize]                                                           // Authorize attribute so that we require authentication              // to test our 401 Unauthorised responses
        [HttpGet("auth")]                                               // when we access this endpoint we go to API BuggyController: "auth"
        public ActionResult<string> GetSecret()         // simple method that returns a string .. called GetSecret():       // here we generate several methods that are not successful
        {
            return "secret text";
        }

        [HttpGet("not-found")]                                               // when we access this endpoint we go to API BuggyController: "auth"
        public ActionResult<AppUser> GetNotFound()     // simple method that returns a string .. called GetSecret():       // here we generate several methods that are not successful   // NB: we are returning an AppUser
        {
            var thing = _context.Users.Find(-1);            // get something here .. we know -1 does not exist to generate an error, no users with ID -1
            if (thing == null) return NotFound();             // we check status of thing .. if true, return NotFound()

            return Ok(thing);                                       // if false .. we find something .. return it
        }

        [HttpGet("server-error")]                                               // when we access this endpoint we go to API BuggyController: "auth"
        public ActionResult<string> GetServerError()          // here we try to generate an exception from this method
        {
            var thing = _context.Users.Find(-1);
            var thingToReturn = thing.ToString();                   // try to convert our thing to a string // null will be return here and we try to execute convert to string to null // null reference exception will be generated

            return thingToReturn;                                       // if we find the null reference exception return it here
        }

        [HttpGet("bad-request")]                                               // when we access this endpoint we go to API BuggyController: "auth"
        public ActionResult<string> GetBadRequest()         // simple method that returns a string .. GetBadRequest():       // here we generate several methods that are not successful
        {
            return BadRequest("This was not a good request");           // simply returns text "this was not a good request"
        }

    }
}