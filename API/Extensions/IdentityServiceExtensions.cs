using System.Text; //Encoding
using Microsoft.AspNetCore.Authentication.JwtBearer; //JwtBearerDefaults
using Microsoft.Extensions.Configuration; //IConfiguration
using Microsoft.Extensions.DependencyInjection; //IServiceCollection
using Microsoft.IdentityModel.Tokens; //TokenValidationParameters

namespace API.Extensions
{
    public static class IdentityServiceExtensions       // needs to be static as its an extension and won't be creating a new instance of this class
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
        {
             services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {                                                                                                               // we chain configuration on here
                    options.TokenValidationParameters = new TokenValidationParameters
                    {                                                                               // main concern is we can validate users with authentication tokens
                        ValidateIssuerSigningKey = true,            // specify the options we want .. server will assign the tokens here, validate the token will be true
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"])),
                        ValidateIssuer = false,                                     // additional flags for best practice implementation        // issuer of token is API server
                        ValidateAudience = false,                               // audience is angular application
                    };
                });

                return services;
        }
    }
}