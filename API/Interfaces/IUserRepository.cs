using System.Collections.Generic;                                                                                                    //IEnumerable
using System.Threading.Tasks;                                                                                                           // Task
using API.DTOs;                                                                                                                                 // MemberDto
using API.Entities;                                                                                                                                 //AppUser
using API.Helpers;          // PagedList<MemberDto>>    // we created this as PagedList.cs in our helpers folder

namespace API.Interfaces
{
    public interface IUserRepository                                                                                                    // the idea here is to provide only methods  we support for our entities // therefore we specify interface as we are providing signatures only
    {
        void Update(AppUser user);                                                                                                      // Update is not an async method as it only updates the entity framework data to inform tracking has changed
                Task<bool> SaveAllAsync();
        Task<IEnumerable<AppUser>> GetUserAsync();
        Task<AppUser> GetUserByIdAsync(int id);
        Task<AppUser> GetUserByUsernameAsync(string username);
                                                                                                                                                                        // we create additional methods to see what our options are for optimisation after establishing our repository and returning main user photos
        //Task<IEnumerable<MemberDto>> GetMembersAsync();                                                                         //returns a list of MemberDto      // insead of returning appUsers
        Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams);        //returns a list of MemberDto      // insead of returning appUsers  // changed to PagedList<>>
        Task<MemberDto> GetMemberAsync(string username);
      //  Task GetUserByUsernameAsync(object p);                                                                                                    // removed as we have this reworked above
    }
}