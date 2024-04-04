using System.Net;

namespace Visualizesse.Domain.Exceptions;

public class UnauthorizedException : Exception
{
    public UnauthorizedException(string message, HttpStatusCode statusCode) : base(message)
    {
        StatusCode = statusCode;
    }
    
    public UnauthorizedException(string message, string exception, HttpStatusCode statusCode) : base(message)
    {
        Exception = exception;
        StatusCode = statusCode;
    }
    
    public string? Exception { get; private set; }
    public HttpStatusCode StatusCode { get; private set; }
}