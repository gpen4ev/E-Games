﻿using AutoMapper;
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
        }
    }
}
