using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class AppUser
    {
        public int Id { get; set; }                                         // Id is a key word field for our database and references Id as key field
        public string UserName { get; set; }                // so we don't confilct with Username that we may use later on in the project

        public byte[] PasswordHash { get; set; }              // byteArrays to be stored as hashes in our database

        public byte[] PasswordSalt { get; set; }                // both byte array properties will be added as columns in our migration once we re-create our table via migrations
    }
}
