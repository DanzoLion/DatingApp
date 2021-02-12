using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;   // module, view, controller
namespace API.Controllers
{
    [Authorize]
    public class LikesController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        // private readonly IUserRepository _unitOfWork.UserRepository;          // removed with UnitOfWork Pattern
        // private readonly ILikesRepository _unitOfWork.LikesRepository;
        // public LikesController(IUserRepository userRepository, ILikesRepository likesRepository)
        public LikesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            // _likesRepository = likesRepository;                               // removed with UnitOfWork Pattern
            // _userRepository = userRepository;
        }
      [HttpPost("{username}")]  // here we place the method for our user to like another user / username will be the user they like
      public async Task<ActionResult> AddLike(string username)
      {
          var sourceUserId = User.GetUserId();
          var likedUser = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);  // here we get hold of the user we like
          var sourceUser = await _unitOfWork.LikesRepository.GetUserWithLikes(sourceUserId);
          if (likedUser == null) return NotFound();
          if (sourceUser.UserName == username) return BadRequest("You cannot like yourself");
        var userLike = await _unitOfWork.LikesRepository.GetUserLike(sourceUserId, likedUser.Id);
        if (userLike != null) return BadRequest("You already like this user");
        userLike = new UserLike    // if none of the above, will create a new userLike
        {
            SourceUserId = sourceUserId,                // these are for our two columns
            LikedUserId = likedUser.Id
        };
        sourceUser.LikedUsers.Add(userLike);
      //  if (await _unitOfWork.UserRepository.SaveAllAsync()) return Ok();                 // removed with UnitOfWork Pattern
        if (await _unitOfWork.Complete()) return Ok();
        return BadRequest("Failed to like user!");
      }  
    [HttpGet]
    //ublic async Task<ActionResult<IEnumerable<LikeDto>>> GetUserLikes(string predicate)
    public async Task<ActionResult<IEnumerable<LikeDto>>> GetUserLikes([FromQuery]LikesParams likesParams)
    {
        likesParams.UserId = User.GetUserId();
       // var users =  await _unitOfWork.LikesRepository.GetUserLikes(predicate, User.GetUserId());
        var users =  await _unitOfWork.LikesRepository.GetUserLikes(likesParams);
    Response.AddPaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);
        return Ok(users);
    }
    }
}