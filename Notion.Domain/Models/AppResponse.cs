using System.Net;
using Notion.Domain.Exceptions;

namespace Notion.Domain.Models
{
    public class AppResponse
    {
        public AppResponse(HttpStatusCode statusCode, IEnumerable<AppError>? errors)
        {
            StatusCode = (int)statusCode;
            Errors = errors;
        }
        
        public AppResponse(int statusCode, IEnumerable<AppError>? errors)
        {
            StatusCode = statusCode;
            Errors = errors;
        }

        public AppResponse(HttpStatusCode statusCode)
        {
            StatusCode = (int)statusCode;
            Errors = null;
        }
        
        public static AppResponse CreateWithOneMessage(IAppException exception)
        {
            return new AppResponse(exception.StatusCode, exception.Errors);
        }
        
        public int StatusCode { get; private set; }
        
        public IEnumerable<AppError>? Errors { get; private set; }
    }

    public class AppResponse<TData> : AppResponse
    {
        public AppResponse(HttpStatusCode statusCode, TData data) : base(statusCode)
        {
            Data = data;
        }
        
        public TData Data { get; private set; }
    }
}
