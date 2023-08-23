using AutoMapper;
using Notion.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Notion.Domain.Shared;

namespace Notion.WebApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureBadRequestResponse(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (context) =>
                {
                    var mapper = context.HttpContext.RequestServices.GetRequiredService<IMapper>();
                    options.SuppressModelStateInvalidFilter = true;

                    return new BadRequestObjectResult(mapper.Map<AppResponse>(context.ModelState));
                };
            });
        }
    }
}