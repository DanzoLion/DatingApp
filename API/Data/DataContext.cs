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
    }                                                                                                                                                        // we then need to add this configuration to our startup class so we inject data context class into our application
}
