using Notion.Domain.Models;

namespace Notion.Domain.Exceptions;

public interface IAppException
{
    int StatusCode { get; }
    
    IEnumerable<AppError>? Errors { get; }
}