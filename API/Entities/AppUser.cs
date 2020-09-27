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
    }
}
