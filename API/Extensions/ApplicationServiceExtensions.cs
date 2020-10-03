using API.Data; //DataContext
using API.Interfaces;   // ITokenService
using API.Services; //TokenService
using Microsoft.EntityFrameworkCore;    //UseSqlite
using Microsoft.Extensions.Configuration;   // IConfig
using Microsoft.Extensions.DependencyInjection; // IServiceCollection

namespace API.Extensions
{
    public static class ApplicationServiceExtensions            // must be static for ApplicationServiceExtension, static means we do not need to create a new instance of this class to use it
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)     // where we extend IServiceCollection in Startup.cs // AddApplicationServices is the name of the method we will be using // to extend use this
        {
                services.AddScoped<ITokenService, TokenService>();                                                                             // added interface scoped to keep alive with HTTP req. for creating Token service
            services.AddDbContext<DataContext>(options =>                                                                               // lambda expression to pass expression as parameter
            {
                options.UseSqlite(config.GetConnectionString("DefaultConnection"));                                            // connection string is what we are passing to our sql database as a connection string
            });

            return services;                                                                        // our services to be returned                                                                                                                                         
        }
    }
}