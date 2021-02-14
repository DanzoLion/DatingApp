using System;
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
                services.AddScoped<IUnitOfWork, UnitOfWork>();                           // added with UnitOfWork  // takes care of our repositories // passes single instance to each of our repositories
        //        services.AddScoped<ILikesRepository, LikesRepository>();                  // likes repository is the implementation class  // removed with UnitOfWork
         //       services.AddScoped<IMessageRepository, MessageRepository>();     // implemented after we create the infrastructure for Message.cs / MessageRepository.cs / MessageDto.cs etc   // removed with UnitOfWork
       //         services.AddScoped<IUserRepository, UserRepository>();               // Adds user repository to AddScoped implementation // added after UserRepository.cs created  // removed with UnitOfWork
                services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);                                                    // added service from AutoMapperProfiles.cs
                services.AddDbContext<DataContext>(options =>                                                                               // lambda expression to pass expression as parameter
            {
               // options.UseSqlite(config.GetConnectionString("DefaultConnection"));                                            // connection string is what we are passing to our sql database as a connection string
            //    options.UseNpgsql(config.GetConnectionString("DefaultConnection"));   // removed with Heroku deployment     // connection string is what we are passing to our sql database as a connection string
          //  {
    var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

    string connStr;

    // Depending on if in development or production, use either Heroku-provided
    // connection string, or development connection string from env var.
    if (env == "Development")
    {
        // Use connection string from file.
        connStr = config.GetConnectionString("DefaultConnection");
    }
    else
    {
        // Use connection string provided at runtime by Heroku.
        var connUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

        // Parse connection URL to connection string for Npgsql
        connUrl = connUrl.Replace("postgres://", string.Empty);
        var pgUserPass = connUrl.Split("@")[0];
        var pgHostPortDb = connUrl.Split("@")[1];
        var pgHostPort = pgHostPortDb.Split("/")[0];
        var pgDb = pgHostPortDb.Split("/")[1];
        var pgUser = pgUserPass.Split(":")[0];
        var pgPass = pgUserPass.Split(":")[1];
        var pgHost = pgHostPort.Split(":")[0];
        var pgPort = pgHostPort.Split(":")[1];

        connStr = $"Server={pgHost};Port={pgPort};User Id={pgUser};Password={pgPass};Database={pgDb};SSL Mode=Require;TrustServerCertificate=True";
    }

    // Whether the connection string came from the local development configuration file
    // or from the environment variable from Heroku, use it to set up your DbContext.
    options.UseNpgsql(connStr);
            });

            return services;                                                                        // our services to be returned                                                                                                                                         
        }
    }
}