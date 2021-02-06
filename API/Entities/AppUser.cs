using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Extensions; // CalculateAge()
using Microsoft.AspNetCore.Identity;

namespace API.Entities
{
    public class AppUser : IdentityUser<int>                   // Identity User for Identity Management with key typ int for user
    {
        // public int Id { get; set; }                                         // Id is a key word field for our database and references Id as key field            // removed IdentityManagement
        // public string UserName { get; set; }                // so we don't confilct with Username that we may use later on in the project          // removed IdentityManagement
        // public byte[] PasswordHash { get; set; }              // byteArrays to be stored as hashes in our database     // removed IdentityManagement
        // public byte[] PasswordSalt { get; set; }                // both byte array properties will be added as columns in our migration once we re-create our table via migrations          // removed IdentityManagement
        public DateTime DateOfBirth { get; set; }                   // DOB value
        public string KnownAs { get; set; }                                     // another name the user is know by
        public DateTime Created { get; set; } = DateTime.Now;                   // as when they created their profie
        public DateTime LastActive { get; set; } = DateTime.Now;            // last time the user was active
        public string Gender { get; set; }                                                                  // SEX -> used to display members of the opposite gender
        public string Introduction { get; set; }                                                // simple intro text
        public string LookingFor { get; set; }                                              // simple text
        public string Interests { get; set; }                                                       // simple text about 
        public string City { get; set; }
        public string Country { get; set; }
        public ICollection<Photo> Photos { get; set; }                                  // we auto generated this class from Photo -> Photo.cs  // first half of defined relationship between AppUser.cs and Photo.cs
        public ICollection<UserLike> LikedByUsers { get; set; }               // users liked by other users
        public ICollection<UserLike> LikedUsers { get; set; }                  // users that have been liked
        public ICollection<Message> MessagesSent { get; set; }                 
        public ICollection<Message> MessagesReceived { get; set; }                  
        public ICollection<AppUserRole> UserRoles {get; set;}           // acts as our join table

//public int GetAge()                                                       // commented out as we have a new implementation for GetMember Dto in UserControllers.cs
//{
//    return DateOfBirth.CalculateAge();                                            // we now have the ability to return somones age via DOB via the entity // relationship between AppUser.cs and Photo.cs = one to many relationship 1 user has many photos
//}

    }
}
