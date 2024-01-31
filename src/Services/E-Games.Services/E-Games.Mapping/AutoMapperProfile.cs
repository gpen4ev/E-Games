using AutoMapper;
using E_Games.Common.DTOs;
using E_Games.Data.Data.Models;

namespace E_Games.Web.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ApplicationUser, UserProfileModelDto>();
            CreateMap<UpdateUserModelDto, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.AddressDelivery, opt => opt.MapFrom(src => src.AddressDelivery));

            CreateMap<Product, FullProductInfoDto>();

            CreateMap<CreateProductDto, Product>()
                .ForMember(dest => dest.Logo, opt => opt.MapFrom(src => src.Logo))
                .ForMember(dest => dest.Background, opt => opt.MapFrom(src => src.Background));
            CreateMap<Product, CreateProductDto>();

            CreateMap<UpdateProductDto, Product>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<Product, UpdateProductDto>();
        }
    }
}
