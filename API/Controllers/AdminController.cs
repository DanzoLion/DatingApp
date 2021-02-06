using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.X86;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers {
    public class AdminController : BaseApiController {
        private readonly UserManager<AppUser> _userManager;
        public AdminController (UserManager<AppUser> userManager) {
            _userManager = userManager;
        }

        [Authorize (Policy = "RequireAdminRole")] // policy name
        [HttpGet ("users-with-roles")] // route

        public async Task<ActionResult> GetUsersWithRoles () // method name
        {
           var users = await _userManager.Users
           .Include(r => r.UserRoles)
           .ThenInclude(r => r.Role)
           .OrderBy(u => u.UserName)
           .Select(u => new                                                                     // returns an object with, u.Id, Username, Roles they are in
           {
               u.Id,                                                            
               Username = u.UserName,
               Roles = u.UserRoles.Select(r => r.Role.Name).ToList()
           })
           .ToListAsync();
        
        return Ok(users);
        }

        [HttpPost("edit-roles/{username}")]                                             // method created to allow admin user to edit roles
        public async Task<ActionResult> EditRoles(string username, [FromQuery] string roles) // we get the string of our roles FromQuery string
        {
            var selectedRoles = roles.Split(",").ToArray();
            var user = await _userManager.FindByNameAsync(username);
            if(user == null) return NotFound("Could not find user");

            var userRoles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));

            if (!result.Succeeded) return BadRequest("Failed to add to roles");
            result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));

            if(!result.Succeeded) return BadRequest("Failed to remove from roles");
            return Ok(await _userManager.GetRolesAsync(user));
        }


        [Authorize (Policy = "ModeratePhotoRole")] // policy name
        [HttpGet ("photos-to-moderate")] // route

        public ActionResult GetPhotosForModerations () // method name
        {
            return Ok ("Admins or moderators can see this");
        }
    }
}