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
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Id))
                .ForMember(dest => dest.PhotoUrl, 
                    opt => opt.MapFrom(
                        src => src.Photos != null ? src.Photos.First().Url : ""));
        }
    }
}
