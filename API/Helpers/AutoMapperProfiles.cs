using System.Linq; // FirstOrDefault
using API.DTOs; //MemberDto
using API.Entities; //AppUser
using API.Extensions;   //CalculateAge()
using AutoMapper; //Profile

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, MemberDto>()           // we specify where we want to map from and where we are mapping to // takes care of mapping from app user to our member
            .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.Photos.FirstOrDefault(x => x.IsMain).Url))
            .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));            // this is our new mapping configuration .. for checking optimisation 
            // we are adding extra configuratio here to populate photoUrl ..
            // we provide dest.PhotoUrl as detination property // we then map from opt.MapFrom(src -> source of where we are mapping from)  -> that goes into users photo collection, gets default that is main.url
            CreateMap<Photo, PhotoDto>();               // mapping from photo to photoDto
                                                                    // this is added as a dependency we need to inject so we need to add this to our ApplicationServiceExtensions.cs
            CreateMap<MemberUpdateDto, AppUser>();    // going from MemberUpdateDto, to AppUser // user .ReverseMap to swap directions
        }
    }
}