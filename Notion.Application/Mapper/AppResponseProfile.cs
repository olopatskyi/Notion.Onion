using AutoMapper;
using Notion.Domain.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Notion.Application.Mapper;

public class AppResponseProfile : Profile
{
    public AppResponseProfile()
    {
        CreateMap<ModelStateDictionary, AppResponse>()
            .ConvertUsing<ModelStateConverter>();
    }
}