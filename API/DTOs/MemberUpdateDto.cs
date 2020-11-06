namespace API.DTOs
{
    public class MemberUpdateDto
    {   // these are the only items we are going to ask our user for // the only properties we req.
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}   // these are not items we ask the user for when they register so we don't need to validate these properties // ie if removed they will all be blanks
// we then need to map this Dto to our user Entity