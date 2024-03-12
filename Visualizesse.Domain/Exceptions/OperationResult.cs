using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Visualizesse.Domain.Exceptions;

public class OperationResult
{
    public bool Success { get; protected set; }
    public string Message { get; protected set; }
    public string Exception { get; protected set; }
    public object Data { get; protected set; }
    public HttpStatusCode StatusCode { get; protected set; }
    
    protected OperationResult(HttpStatusCode statusCode)
    {
        Success = true;
        StatusCode = statusCode;
    }
    
    protected OperationResult(object data, HttpStatusCode statusCode)
    {
        Success = true;
        StatusCode = statusCode;
        Data = data;
    }

    protected OperationResult(string message, HttpStatusCode statusCode)
    {
        Success = false;
        Message = message;
        StatusCode = statusCode;
    }

    protected OperationResult(string exception, string? message, HttpStatusCode statusCode)
    {
        Success = false;
        Exception = exception;
        Message = message;
        StatusCode = statusCode;
    }
    
    public static OperationResult SuccessResult(HttpStatusCode statusCode)
    {
        return new OperationResult(statusCode);
    }
    
    public static OperationResult SuccessResult(object data, HttpStatusCode statusCode)
    {
        return new OperationResult(data, statusCode);
    }
    
    
    public static OperationResult FailureResult(string message, HttpStatusCode statusCode)
    {
        return new OperationResult(message, statusCode);
    }
    public static OperationResult ExceptionResult(string ex, string message, HttpStatusCode statusCode)
    {
        return new OperationResult(ex, message, statusCode);
    }
    
    public static OperationResult ExceptionResult(string message, HttpStatusCode statusCode)
    {
        return new OperationResult(message, statusCode);
    }
    public bool IsException()
    {
        return Exception != null;
    }
}