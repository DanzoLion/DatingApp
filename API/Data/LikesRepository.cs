using System.Data.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;
using API.Extensions;
using API.Helpers;

namespace API.Data {
    public class LikesRepository : ILikesRepository {
        private readonly DataContext _context;
        public LikesRepository (DataContext context) {
            _context = context;
        }

        public async Task<UserLike> GetUserLike (int sourceUserId, int likedUserId) {
            return await _context.Likes.FindAsync(sourceUserId, likedUserId);             // these two items make up our primary key
        }

        //public async Task<IEnumerable<LikeDto>> GetUserLikes(string predicate, int userId) {    // our predicate means we add our queryables here
        // public async Task<PagedList<LikeDto>> GetUserLikes(string predicate, int userId) {    // our predicate means we add our queryables here
        public async Task<PagedList<LikeDto>> GetUserLikes(LikesParams likesParams) {    // our predicate means we add our queryables here
           var users = _context.Users.OrderBy(u => u.UserName).AsQueryable();                             // this is our join query that Entity Framework Works out for us
           var likes = _context.Likes.AsQueryable();

            if (likesParams.Predicate == "liked"){
                likes = likes.Where(like => like.SourceUserId == likesParams.UserId);
                users = likes.Select(like => like.LikedUser);
            }
            
            if (likesParams.Predicate == "likedBy"){
                likes = likes.Where(like => like.LikedUserId == likesParams.UserId);
                users = likes.Select(like => like.SourceUser);
            }

            var likedUsers = users.Select(user => new LikeDto{
                Username = user.UserName,
                KnownAs = user.KnownAs,
                Age = user.DateOfBirth.CalculateAge(),
                PhotoUrl = user.Photos.FirstOrDefault(p => p.IsMain).Url,
                City = user.City,
                Id = user.Id
            //}).ToListAsync();
            });

            return await PagedList<LikeDto>.CreateAsync(likedUsers, likesParams.PageNumber, likesParams.PageSize);
        }

        public async Task<AppUser> GetUserWithLikes (int userId) {
            return await _context.Users.Include(x => x.LikedUsers).FirstOrDefaultAsync(x => x.Id == userId);
        }
    }
}