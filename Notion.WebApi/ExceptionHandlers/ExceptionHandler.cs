
using ExceptionHandler;

namespace Notion.WebApi.ExceptionHandlers;

public class ExceptionHandler : IExceptionHandler<Exception>
{
    public async Task ProceedAsync(HttpContext context, Exception exception)
    {
        context.Response.StatusCode = 500;
        await context.Response.WriteAsJsonAsync(new
        {
            error = exception.Message
        });
    }
}