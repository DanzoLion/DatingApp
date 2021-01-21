namespace API.Entities
{
    public class UserLike
    {
        public AppUser SourceUser { get; set; }    // user that likes other user
        public int SourceUserId { get; set; }
        public AppUser LikedUser { get; set; }    // users that have liked a user 
        public int LikedUserId { get; set; }

    }
}