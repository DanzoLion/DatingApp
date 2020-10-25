using System; // DateTime
using System.Collections.Generic; //ICollection

namespace API.DTOs
{
    public class MemberDto                                                                                                            // contents copied from AppUser.cs
    {   
                public int Id { get; set; }                                                                                          // Id is a key word field for our database and references Id as key field
        public string UserName { get; set; }                                                                                // so we don't confilct with Username that we may use later on in the project
    //    public byte[] PasswordHash { get; set; }                                                                      // byteArrays to be stored as hashes in our database
    //    public byte[] PasswordSalt { get; set; }                                                                        // both byte array properties will be added as columns in our migration once we re-create our table via migrations
    //   public DateTime DateOfBirth { get; set; }                                                                 // DOB value

        public string PhotoUrl { get; set; }                                                              // added after AutoMapper created and mapped // this will set our main photo to main photo collection will be the property we send back to our user for main photo

        public int Age { get; set;}
        public string KnownAs { get; set; }                                                                               // another name the user is know by
        public DateTime Created { get; set; } //= DateTime.Now;                                        // as when they created their profie
        public DateTime LastActive { get; set; }// = DateTime.Now;                                     // last time the user was active
        public string Gender { get; set; }                                                                            // SEX -> used to display members of the opposite gender
        public string Introduction { get; set; }                                                                      // simple intro text
        public string LookingFor { get; set; }                                                                        // simple text
        public string Interests { get; set; }                                                                             // simple text about 
        public string City { get; set; }
        public string Country { get; set; }
       // public ICollection<Photo> Photos { get; set; }                                                       // we auto generated this class from Photo -> Photo.cs  // first half of defined relationship between AppUser.cs and Photo.cs
       public ICollection<PhotoDto> Photos {get; set;}                                                  // created PhotoDto.cs class from here -> added Id/Url/IsMain
    }
}