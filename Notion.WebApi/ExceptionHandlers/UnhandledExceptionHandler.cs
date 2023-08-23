using ExceptionHandler;
using Notion.Domain.Exceptions;
using Notion.Domain.Models;

namespace Notion.WebApi.ExceptionHandlers;

public class UnhandledExceptionHandler : IExceptionHandler<UnhandledException>
{
    public async Task ProceedAsync(HttpContext context, UnhandledException exception)
    {
        context.Response.StatusCode = exception.StatusCode;
        await context.Response.WriteAsJsonAsync(AppResponse.CreateWithOneMessage(exception));
    }
}