using ExceptionHandler;
using Notion.Domain.Exceptions;
using Notion.Domain.Models;

namespace Notion.WebApi.ExceptionHandlers;

public class ValidationExceptionHandler : IExceptionHandler<ValidationException>
{
    public async Task ProceedAsync(HttpContext context, ValidationException exception)
    {
        context.Response.StatusCode = exception.StatusCode;
        await context.Response.WriteAsJsonAsync(AppResponse.CreateWithOneMessage(exception));
    }
}