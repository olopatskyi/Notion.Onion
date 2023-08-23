using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Notion.Domain.Models;

namespace Notion.Application.Mapper;

public class ModelStateConverter : ITypeConverter<ModelStateDictionary, AppResponse>
{
    public AppResponse Convert(ModelStateDictionary source, AppResponse destination, ResolutionContext context)
    {
        var errors = new List<AppError>();
        foreach (var state in source)
        {
            foreach (var value in state.Value.Errors)
            {
                errors.Add(new AppError(state.Key, value.ErrorMessage));
            }
        }

        return new AppResponse(HttpStatusCode.BadRequest, errors);
    }
}