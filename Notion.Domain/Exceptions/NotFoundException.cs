using Notion.Domain.Models;

namespace Notion.Domain.Exceptions;

public class NotFoundException : Exception, IAppException
{
    public NotFoundException(IEnumerable<AppError> errors)
    {
        Errors = errors;
    }

    public NotFoundException(string error)
    {
        Errors = new[] { new AppError(null, error) };
    }
    
    public int StatusCode => 404;
    
    public IEnumerable<AppError>? Errors { get; }

    public static NotFoundException Default<TResource>(TResource resource)
    {
        var resourceName = typeof(TResource).Name;

        return new NotFoundException($"{resourceName} not found");
    }
}