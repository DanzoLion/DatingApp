using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;
using System.Transactions;
using System.Text; //Encoding
using Microsoft.AspNetCore.Authentication.JwtBearer; //JwtBearerDefaults
using Microsoft.Extensions.Configuration; //IConfiguration
using Microsoft.Extensions.DependencyInjection; //IServiceCollection
using Microsoft.IdentityModel.Tokens; //TokenValidationParameters
using API.Entities;
using Microsoft.AspNetCore.Identity;
using API.Data;

namespace API.Extensions
{
    public static class IdentityServiceExtensions       // needs to be static as its an extension and won't be creating a new instance of this class
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddIdentityCore<AppUser>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = false;
            })
                .AddRoles<AppRole>()
                .AddRoleManager<RoleManager<AppRole>>()
                .AddSignInManager<SignInManager<AppUser>>()
                .AddRoleValidator<RoleValidator<AppRole>>()
                .AddEntityFrameworkStores<DataContext>();
            
            
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

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];    // allows client to send up token as a query string  // access_token is a key
                            var path = context.HttpContext.Request.Path;

                            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))           // checks to see if we have an access token
                            {
                                context.Token = accessToken;
                            }

                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
                opt.AddPolicy("ModeratePhotoRole", policy => policy.RequireRole("Admin", "Moderator"));
            });

                return services;
        }
    }
}