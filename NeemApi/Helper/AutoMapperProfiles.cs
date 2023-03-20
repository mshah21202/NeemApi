using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NeemApi.DTOs;
using NeemApi.Entities;
using System.ComponentModel.DataAnnotations;

namespace NeemApi.Helper
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<RegisterDto, AppUser>();
            CreateMap<AppUser, MemberDto>();
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.PhotoUrl, 
                    opt => opt.MapFrom(
                        src => src.Photos != null ? src.Photos.First().Url : ""));
            CreateMap<OrderProduct, ProductDto>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ProductId));
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.Total))
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.OrderProduct));
            CreateMap<Photo, string>()
                .ConvertUsing(src => src.Url);
            CreateMap<Product, ProductDetailDto>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.PhotoUrls, opt => opt.MapFrom(src => src.Photos));
        }
    }
}
