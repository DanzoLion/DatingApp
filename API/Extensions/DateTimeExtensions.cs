using System;
namespace API.Extensions
{
    public static class DateTimeExtensions              // with all extension methods we make these static
    {
        public static int CalculateAge(this DateTime dob)
        {
            var today = DateTime.Today;
            var age = today.Year - dob.Year;                                // this works out their current age
            if(dob.Date > today.AddYears(-age)) age--;              // this check performs if the user has their birthday today
            return age;
        }
    }
}