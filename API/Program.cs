using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;                                                         // <DataContext>
using API.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;                    // MigrateAync()
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection; // CreateScope()
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API
{
    public class Program
    {
        //public static void Main(string[] args)                                          // method to run dotnet commands            // what happens here is the first instance before our application starts
        public static async Task Main(string[] args)                                    // changed to this after Seed.cs created to seed our database.json data     // we are outside of our middleware here
        {
            //CreateHostBuilder(args).Build().Run();                                  // method // adjusted after Seed.cs created to seed our database .json data
            var host = CreateHostBuilder(args).Build();
            using var scope = host.Services.CreateScope();    // need to get our data context service to pass to our seed method // creates a scope for the services we create in this section
            var services = scope.ServiceProvider;
            try         // we need this because we are outside of our global error exception handling boundary
            {
                var context = services.GetRequiredService<DataContext>();
                var userManager = services.GetRequiredService<UserManager<AppUser>>();
                var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
                await context.Database.MigrateAsync();  // migrates our database here // applies any pending migrations for the context to database // creates database if it does not already exist ie > dotnet ef update
                await Seed.SeedUsers(userManager, roleManager);              // if our database is dropped it will re-create the database if it is dropped  // user/context replaced with userManger for identity manager
            }
            catch(Exception ex)            // catches the excepotions that occur during the seeding of our data
            {
                var logger = services.GetRequiredService<ILogger<Program>>();          // get ILogger interface and Program.cs as type
                logger.LogError(ex, "An error occurred during migration");
            }

            await host.RunAsync();

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>                      // configuration method for initialising our program
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
