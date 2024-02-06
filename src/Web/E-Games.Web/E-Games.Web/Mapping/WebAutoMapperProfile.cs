using AutoMapper;
using E_Games.Common.DTOs;
using E_Games.Web.ViewModels;

namespace E_Games.Web.Mapping
{
    public class WebAutoMapperProfile : Profile
    {
        public WebAutoMapperProfile()
        {
            CreateMap<UpdateUserModel, UpdateUserModelDto>();
            CreateMap<UpdatePasswordModel, UpdatePasswordModelDto>();

            CreateMap<PlatformPopularityDto, PlatformPopularity>();

            CreateMap<SearchGameDto, SearchGame>();

            CreateMap<FullProductInfoDto, FullProductInfoModel>();

            CreateMap<CreateProductDto, CreateProductModel>();
            CreateMap<CreateProductModel, CreateProductDto>()
                .ForMember(dest => dest.LogoFile, opt => opt.MapFrom(src => src.LogoFile))
                .ForMember(dest => dest.BackgroundImageFile, opt => opt.MapFrom(src => src.BackgroundImageFile));

            CreateMap<UpdateProductDto, UpdateProductModel>();
            CreateMap<UpdateProductModel, UpdateProductDto>();

            CreateMap<EditRatingDto, EditRatingModel>();
            CreateMap<EditRatingModel, EditRatingDto>();
        }
    }
}
