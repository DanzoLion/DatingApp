using System.Collections.Generic;

namespace API.Entities
{
    public class AppRole : Microsoft.AspNetCore.Identity.IdentityRole<int>
    {
        public ICollection<AppUserRole> UserRoles {get; set;}
}
}