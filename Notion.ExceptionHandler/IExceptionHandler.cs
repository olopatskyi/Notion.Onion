namespace ExceptionHandler;
using Microsoft.AspNetCore.Http;

public interface IExceptionHandler<in TException> where TException : Exception
{
    Task ProceedAsync(HttpContext context, TException exception);
}