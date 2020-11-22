//using System.Security.Claims;   // ClaimsPrinciplal derived from UsersController.cs ControllerBase.User -> var username 

using System.Security.Claims;

namespace API.Extensions
{
    public static class ClaimsPrincipleExtensions
    {
        public static string GetUsername(this ClaimsPrincipal user)                 // here we are using GetUsername on the user instead of typing it all out ..
        {
            return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}

