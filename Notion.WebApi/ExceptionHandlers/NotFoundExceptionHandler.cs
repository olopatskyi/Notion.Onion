using ExceptionHandler;
using Notion.Domain.Exceptions;
using Notion.Domain.Models;

namespace Notion.WebApi.ExceptionHandlers;

public class NotFoundExceptionHandler : IExceptionHandler<NotFoundException>
{
    public async Task ProceedAsync(HttpContext context, NotFoundException exception)
    {
        context.Response.StatusCode = exception.StatusCode;
        await context.Response.WriteAsJsonAsync(AppResponse.CreateWithOneMessage(exception));
    }
}