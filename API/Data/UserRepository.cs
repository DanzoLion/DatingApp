using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore; //SingleOrDefaultAsync()
using API.DTOs;
using System.Linq;
using AutoMapper.QueryableExtensions;
using AutoMapper;                   // IMapper
using API.Helpers;                      // UserParams

namespace API.Data
{
    public class UserRepository : IUserRepository // implement interface from IUserRepository for AutoFill

    {
        private readonly DataContext _context;          // we need a _context field/variable/parameter/argument for context
        private readonly IMapper _mapper;               // initialised after injecting IMapper mapper
        public UserRepository(DataContext context, IMapper mapper) // User Repository accesses our Db context therefore we need a constructor to do this // injected here to use with MemberDto below
        {
            _mapper = mapper;                                   // initialised after injecting IMapper mapper
            _context = context;                                     // initialised this field from context  
        }

        public async Task<MemberDto> GetMemberAsync(string username)         // implemented these interfaces from IUserRepository.cs
        {
            return await _context.Users.Where(x => x.UserName == username).ProjectTo<MemberDto>(_mapper.ConfigurationProvider).SingleOrDefaultAsync();   // this is where we execute the query and it goes to our database         
            //_mapper.ConfigurationProvided added after we injected IMapper mapper and initialised _mapper    // gets the configuration we provided in our automapper profile

            // This is our First Option .. but there is a better way than this ..
            //   .Select(user => new MemberDto                    // within the select statement we maually map the properties we need from our database // that we put inside and return from our MemberDto
            // Id = user.Id,
            //    UserName = user.UserName 
        }

        public async Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams) // changed from IEnumerable to PagedList // implemented interface from IsuerRepository.cs // need to change this to async after adjusting this Task // UserParams added IUserRepository addition
        {
          // return await _context.Users        // altered with the addition of UserParams adaptation with this class
        //   var query =  _context.Users      // this is now an expression tree ie type IQueryable .. entity framework builds this as an expression tree // when async is executed the req. is executed in our database
        var query = _context.Users.AsQueryable();
        /*     .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)  // ProjectTo selects all of users
             .AsNoTracking()   // we are only going to read from these entities, we won't do anything with them other than read them so we can specify no tracking for these
         //  .ToListAsync();       // this will execute the database query  // removed with the additionof UserParams class
             .AsQueryable();*/
                                                        // below we can do something with this query // and we have built up our expression tree
        query = query.Where(u => u.UserName != userParams.CurrentUsername);  // where we return all the users except the currently logged in user
        query = query.Where(u => u.Gender == userParams.Gender);            

        // the below age filter selects the parameters entered, ie 30 && 50 and uses these as minDob and maxDob
        var minDob = DateTime.Today.AddYears((- userParams.MaxAge) -1);  // -1 becuase based on todays date, and to give accurate year
        var maxDob = DateTime.Today.AddYears(- userParams.MinAge);

        query = query.Where(u => u.DateOfBirth >= minDob && u.DateOfBirth <= maxDob);
        query = userParams.OrderBy switch                                                           // new version of switch statement without cases, "created" is a case and _ is a default case
        {
            "created" => query.OrderByDescending(u => u.Created),
            _ => query.OrderByDescending(u => u.LastActive)
        };

         //return await PagedList<MemberDto>.CreateAsync(query, userParams.PageNumber, userParams.PageSize); // .CreateAsync() takese the parameters from PagedList
         return await PagedList<MemberDto>.CreateAsync(query.ProjectTo<MemberDto>(_mapper.ConfigurationProvider).AsNoTracking(), userParams.PageNumber, userParams.PageSize); // we project before we send to our list
         // this method still sends to MemberDto but filters before we get to that point where we use the query methods // CreateAsync method takes care of the execution for us
        }

        public async Task<IEnumerable<AppUser>> GetUserAsync()
        {
            return await _context.Users
            .Include(p => p.Photos)                                                                     // this will include our related array of photos included in our response // multiline formate
            .ToListAsync();
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {                              // NB: we need to make each of these methods async
            return await _context.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.Include(p => p.Photos).SingleOrDefaultAsync(x => x.UserName == username);       // // this will include our related array of photos included in our response // single line format
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;                                           // where we specify greater than zero changes are to be made and saved to our database
        }

        public void Update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;                                  // informs entity framework that the state has been modified // by adding a flag to the state to say True it has been modified
        }
    }
}