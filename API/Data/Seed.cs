using System.Text;
using System.Security.Cryptography; //HMAC
using System.Collections.Generic; // List
using System.Text.Json; // JsonSerialiser
using System.Threading.Tasks; // Task
using API.Entities; // AppUser
using Microsoft.EntityFrameworkCore; // AnyAsync()

namespace API.Data                                                                      // we call paramater from our SeedUsers() method ie DataContext context in Program.cs when we start our applcation // where we seed our data
{
    public class Seed
    {
        public static async Task SeedUsers(DataContext context)                                                  // static method created so we don't need to use a new instance of class Seed // SeedUsers name of the method // Task returns void but provides async functionality
        {
            if (await context.Users.AnyAsync()) return;                                                                     // checks for users and will return from this if we have users

            var userData = await System.IO.File.ReadAllTextAsync("Data/UserSeedData.json");    // if we don't have users in our database we want to interrogate our file and store what we have and store into a variable // gets data from location
            var users = JsonSerializer.Deserialize<List<AppUser>>(userData);                             // users should be of type normal app users extracted from .json file
            foreach (var user in users)                                                                                             // we can now add relavant user Name and Password details for our users with encoding
            {
                using var hmac = new HMACSHA512();

                user.UserName = user.UserName.ToLower();
                user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"));
                user.PasswordSalt = hmac.Key;

                context.Users.Add(user);                                                                                        // we are tracking here, adding tracking through entity framework // nothing is manipulated with the database yet here
            }

            await context.SaveChangesAsync();                                                                          // now we can await, as once our loop finishes we save the changes                                                                     
        }
    }
}