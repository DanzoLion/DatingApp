using API.Entities;                             // for parameter AppUser

namespace API.Interfaces
{
    public interface ITokenService
    {
         string CreateToken(AppUser user);   //JWT Tokens are just strings so that's what we will return here // the create implementation class for class service
    }
}