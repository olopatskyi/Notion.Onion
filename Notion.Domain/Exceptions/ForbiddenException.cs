using System.Net;
using Notion.Domain.Models;

namespace Notion.Domain.Exceptions;

public class ForbiddenException : Exception, IAppException
{
    public ForbiddenException(HttpStatusCode statusCode, IEnumerable<AppError>? error)
    {
        StatusCode = (int)statusCode;
        Errors = error;
    }

    public int StatusCode { get; }
    public IEnumerable<AppError>? Errors { get; }

    public static ForbiddenException Default()
    {
        return new ForbiddenException(HttpStatusCode.Forbidden, new[]
        {
            new AppError(null, "You don't have permissions")
        });
    }
}