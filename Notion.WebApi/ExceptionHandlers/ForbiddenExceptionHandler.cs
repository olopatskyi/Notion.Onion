using ExceptionHandler;
using Notion.Domain.Exceptions;
using Notion.Domain.Models;

namespace Notion.WebApi.ExceptionHandlers;

public class ForbiddenExceptionHandler : IExceptionHandler<ForbiddenException>
{
    public async Task ProceedAsync(HttpContext context, ForbiddenException exception)
    {
        context.Response.StatusCode = exception.StatusCode;
        await context.Response.WriteAsJsonAsync(AppResponse.CreateWithOneMessage(exception));
    }
}