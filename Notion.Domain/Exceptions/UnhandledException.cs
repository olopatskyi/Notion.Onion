using Notion.Domain.Models;

namespace Notion.Domain.Exceptions;
public class UnhandledException : Exception, IAppException
{
    public UnhandledException(IEnumerable<AppError> error)
    {
        Errors = error;
    }

    public UnhandledException(string error)
    {
        Errors = new[] { new AppError(null, error) };
    }
    
    public int StatusCode => 500;
    
    public IEnumerable<AppError>? Errors { get; }
}