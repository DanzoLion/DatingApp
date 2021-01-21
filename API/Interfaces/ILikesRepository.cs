using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface ILikesRepository
    {
         Task<UserLike> GetUserLike(int SourceUserId, int LikedUserId);   // this is all we need to return a like
         Task<AppUser> GetUserWithLikes(int userId);   // this is all we need to return a like

        Task<PagedList<LikeDto>> GetUserLikes(LikesParams likesParams);   // adjusted IEnumerable to PagesList for likes filtering
    }
}