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

            CreateMap<CreateProductDto, CreateProducModel>();
            CreateMap<CreateProducModel, CreateProductDto>();

            CreateMap<UpdateProductDto, UpdateProductModel>();
            CreateMap<UpdateProductModel, UpdateProductDto>();
        }
    }
}
