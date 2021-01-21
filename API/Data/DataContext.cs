using API.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace API.Data
{
    public class DataContext : DbContext                                                                                     // here we derive from the DbContext class within Entity Framework
    {                                                                                                                                                       // our constructor below
        public DataContext(DbContextOptions options) : base(options)                  
        {
        }

        public DbSet<AppUser> Users { get; set; }                                                                       // takes the type of class we want to create a Db for // Users is the name of our Db table
                                                                                                                                                // we then need to add this configuration to our startup class so we inject data context class into our application
    public DbSet<UserLike> Likes { get; set; }                             // our liked users for storing in DB
    // we then need to override a method inside DB context so that we can configure our entities: we use protected
   protected override void OnModelCreating(ModelBuilder builder){    // this is the method we are overriding
    base.OnModelCreating(builder);   // base is the class we derive from and we pass in builder to pass the method -> reduces error occurrence when we try add migration
  
  builder.Entity<UserLike>()    // we now work on our entities
  .HasKey(k => new {k.SourceUserId, k.LikedUserId}); // we specify this .HasKey and we will specify and configure this key ourselves; combination of source userId and liked userId; forms the primary key for the database table
  
  builder.Entity<UserLike>() // we then configure the relationshps inside the table here
  .HasOne(s => s.SourceUser)          // first half of our relationship
  .WithMany(l => l.LikedUsers) 
  .HasForeignKey(s => s.SourceUserId)
  .OnDelete(DeleteBehavior.Cascade); 

    builder.Entity<UserLike>() // this is the other side of the relationship
  .HasOne(s => s.LikedUser)
  .WithMany(l => l.LikedByUsers) 
  .HasForeignKey(s => s.LikedUserId)
  .OnDelete(DeleteBehavior.Cascade); 
   }
    }                                                                                                                                                       
}
