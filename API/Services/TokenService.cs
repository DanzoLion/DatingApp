using System;
using System.Collections.Generic;   // Claim
using System.IdentityModel.Tokens.Jwt;  // JwtRegisteredClaimNames
using System.Security.Claims;       // Claim
using System.Text;  
using API.Entities;                         // for AppUser
using API.Interfaces;                   // ITokenService
using Microsoft.Extensions.Configuration; // IConfiguration
using Microsoft.IdentityModel.Tokens; //SymmetricSecurityKey

namespace API.Services
{
    public class TokenService : ITokenService                   // implements ITokenService interface
    {
        private readonly SymmetricSecurityKey _key;         // symmetric key: where single key used to encrypt decrypt electronic data // asymmetric: pair of keys, 1public + 1private used to encrypt data -where HTTPS + HTML works asymmetrically
        
        public TokenService(IConfiguration config)            // generated constructor from TokenService we get our configuration for TokenService from IConfiguration
        {
             _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));   // where we use Encoding to encode our key into a bytearray // will will use TokenKey property shortly
        }

        public string CreateToken(AppUser user)
        {
            var claims = new List<Claim>
            {
            new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),   // added user.Id.ToString()         // use the NameId to store user.UserName // to store the user name // NameId to store user name // single claim used for now
            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),   // added so we can access both user.Id & UserName
            };

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);   // create user credentials here; takes security key and algorithm // HmacSha strongest available to sign the token

            var tokenDescriptor = new SecurityTokenDescriptor                   // sign our token here
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds
            };
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}