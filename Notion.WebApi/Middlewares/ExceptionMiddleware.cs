using ExceptionHandler;
using Notion.Domain.Models;

namespace Notion.WebApi.Middlewares;
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        AppResponse? response = null;
        
        try
        {
            await _next.Invoke(context);
        }
        catch (Exception e)
        {
            var handlerType = typeof(IExceptionHandler<>).MakeGenericType(e.GetType());
            var handler = context.RequestServices.GetService(handlerType);
            
            if (handler != null)
            {
                var method = handler.GetType().GetMethod("ProceedAsync") ?? 
                             throw new InvalidOperationException($"Method ProceedAsync not found.");
                
                var task = (Task)method.Invoke(handler, new object[]{context, e})!;
                await task;
            }
        }

        if (response != null)
        {
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}

public static class Exceptions
{
    public static IApplicationBuilder UseExceptionHandler(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ExceptionMiddleware>();
    } 
}
