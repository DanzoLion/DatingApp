using System.Collections.ObjectModel;
using System.Text;
using System.Security.Cryptography; //HMAC
using System.Collections.Generic; // List
using System.Text.Json; // JsonSerialiser
using System.Threading.Tasks; // Task
using API.Entities; // AppUser
using Microsoft.EntityFrameworkCore; // AnyAsync()
using Microsoft.AspNetCore.Identity;

namespace API.Data                                                                      // we call paramater from our SeedUsers() method ie DataContext context in Program.cs when we start our applcation // where we seed our data
{
    public class Seed
    {
        //public static async Task SeedUsers(DataContext context) // replaced with identity management implementation   // static method created so we don't need to use a new instance of class Seed // SeedUsers name of the method // Task returns void but provides async functionality
        public static async Task SeedUsers(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)                                                  // static method created so we don't need to use a new instance of class Seed // SeedUsers name of the method // Task returns void but provides async functionality
        {
            if (await userManager.Users.AnyAsync()) return;                                                                     // checks for users and will return from this if we have users

            var userData = await System.IO.File.ReadAllTextAsync("Data/UserSeedData.json");    // if we don't have users in our database we want to interrogate our file and store what we have and store into a variable // gets data from location
            var users = JsonSerializer.Deserialize<List<AppUser>>(userData);                             // users should be of type normal app users extracted from .json file

            if (users == null) return;

            var roles = new List<AppRole>                      // role implementation
            {
                new AppRole{Name = "Member"},
                new AppRole{Name = "Admin"},
                new AppRole{Name = "Moderator"},
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            foreach (var user in users)                                                                                             // we can now add relavant user Name and Password details for our users with encoding
            {
   //             using var hmac = new HMACSHA512();                  // removed with identity management

                user.UserName = user.UserName.ToLower();
                // user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"));  // removed with identity management
                // user.PasswordSalt = hmac.Key;
               //  context.Users.Add(user);   removed with identity manager             // we are tracking here, adding tracking through entity framework // nothing is manipulated with the database yet here
                await userManager.CreateAsync(user, "Pa$$w0rd");                                                                                        // we are tracking here, adding tracking through entity framework // nothing is manipulated with the database yet here
                await userManager.AddToRoleAsync(user, "Member");
           }

        var admin = new AppUser{                                            // creates a new user for admin
            UserName = "admin"
        };

        await userManager.CreateAsync(admin, "Pa$$w0rd");
        await userManager.AddToRolesAsync(admin, new[] {"Admin", "Moderator"});
        //    await context.SaveChangesAsync();  // now we can await, as once our loop finishes we save the changes  // removed with identity manager and this takes care of saving changes to db                                                                     
        }
    }
}