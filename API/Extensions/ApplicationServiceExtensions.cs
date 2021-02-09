using API.Data; //DataContext
using API.Helpers;  //AddAutoMapper()
using API.Interfaces;   // ITokenService
using API.Services; //TokenService
using API.SignalR;
using AutoMapper; //AutoMapperProfiles
using Microsoft.EntityFrameworkCore;    //UseSqlite
using Microsoft.Extensions.Configuration;   // IConfig
using Microsoft.Extensions.DependencyInjection; // IServiceCollection

namespace API.Extensions
{
    public static class ApplicationServiceExtensions            // must be static for ApplicationServiceExtension, static means we do not need to create a new instance of this class to use it
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)     // where we extend IServiceCollection in Startup.cs // AddApplicationServices is the name of the method we will be using // to extend use this

        {
                services.AddSingleton<PresenceTracker>();                                                           // added with PresenceTracker.cs implementation
                services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));            // we instruct our config  where to get our cloudinary config settings // CloudinarySettings type // we can access our "CloudinarySettings" via <CloudinarySettings> class
                services.AddScoped<ITokenService, TokenService>();                                                                             // added interface scoped to keep alive with HTTP req. for creating Token service
                services.AddScoped<IPhotoService, PhotoService>();                                                                  // implement this service after creating PhotoService.cs
                services.AddScoped<LogUserActivity>();                                                                  // scoped as we want this service to be scoped to the context of the request
                services.AddScoped<ILikesRepository, LikesRepository>();                            // likes repository is the implementation class
                services.AddScoped<IMessageRepository, MessageRepository>();                // implemented after we create the infrastructure for Message.cs / MessageRepository.cs / MessageDto.cs etc
                services.AddScoped<IUserRepository, UserRepository>();                                                                      // Adds user repository to AddScoped implementation // added after UserRepository.cs created
                services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);                                                    // added service from AutoMapperProfiles.cs
                services.AddDbContext<DataContext>(options =>                                                                               // lambda expression to pass expression as parameter
            {
                options.UseSqlite(config.GetConnectionString("DefaultConnection"));                                            // connection string is what we are passing to our sql database as a connection string
            });

            return services;                                                                        // our services to be returned                                                                                                                                         
        }
    }
}