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

        public async Task<IEnumerable<MemberDto>> GetMembersAsync() // implemented interface from IsuerRepository.cs // need to change this to async after adjusting this Task
        {
           return await _context.Users
           .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)                     // ProjectTo selects all of users
           .ToListAsync();                                                                                           // this will execute the database query
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