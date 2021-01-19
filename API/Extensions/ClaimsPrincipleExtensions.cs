//using System.Security.Claims;   // ClaimsPrinciplal derived from UsersController.cs ControllerBase.User -> var username 

using System.Security.Claims;

namespace API.Extensions
{
    public static class ClaimsPrincipleExtensions
    {
        public static string GetUsername(this ClaimsPrincipal user)                 // here we are using GetUsername on the user instead of typing it all out ..
        {
           //  return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;  NameIdentifier is replaced with Name
            return user.FindFirst(ClaimTypes.Name)?.Value;
        }

        public static int GetUserId(this ClaimsPrincipal user)                 // second extension method added here to GetUserId
        {

            return int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value);  // need to return an integer here so we wrap the return in int.Parse()
        }
    }
}

