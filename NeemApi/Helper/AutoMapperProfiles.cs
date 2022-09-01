using AutoMapper;
using NeemApi.DTOs;
using NeemApi.Entities;

namespace NeemApi.Helper
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<RegisterDto, AppUser>();
            CreateMap<AppUser, MemberDto>();
        }
    }
}
