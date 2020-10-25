using System.ComponentModel.DataAnnotations.Schema;  // [Table("Photos")]

namespace API.Entities
{
    [Table("Photos")]                                                   // photo entity called ("Photos") for our database         // attribute table created here    // entity framework when creating the table will call the table photos                      
    public class Photo                                                      // we generated this class from AppUser.cs and generated class from public ICollection<Photo> Photos { get; set; }   
    {
        public int Id { get; set; }                                                         // unique ID of photo
        public string Url { get; set; }
        public bool IsMain { get; set; }                                // to check for main photo
        public string PublicId { get; set; }                        // used for the photo storage solution we are going to implement
        public AppUser AppUser { get; set; }                //1. created to fully define relationship between AppUser.cs & Photo.cs
        public int AppUserId { get; set; }                      //2. second part of defining relationship between AppUser.cs & Photo.cs     // the second half of the relationship where we have one user with many photos defined inside class Photo

    }
}