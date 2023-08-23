using Notion.Domain.Models;

namespace Notion.Domain.Exceptions;

public class ValidationException : Exception, IAppException
{
    public ValidationException(IEnumerable<AppError> error)
    {
        Errors = error;
    }

    public ValidationException(string error)
    {
        Errors = new[] { new AppError(null, error) };
    }
    
    public int StatusCode => 400;
    
    public IEnumerable<AppError>? Errors { get; }
}