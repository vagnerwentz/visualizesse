using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Visualizesse.Domain.Exceptions;

public class OperationResult
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public string Exception { get; set; }
    public object Data { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    
    // Adicionando um construtor público padrão
    public OperationResult() {}
    
    public OperationResult(HttpStatusCode statusCode)
    {
        Success = true;
        StatusCode = statusCode;
    }
    
    public OperationResult(object data, HttpStatusCode statusCode)
    {
        Success = true;
        StatusCode = statusCode;
        Data = data;
    }

    public OperationResult(string message, HttpStatusCode statusCode, bool success = false)
    {
        Success = success;
        Message = message;
        StatusCode = statusCode;
    }

    public OperationResult(string exception, string? message, HttpStatusCode statusCode)
    {
        Success = false;
        Exception = exception;
        Message = message;
        StatusCode = statusCode;
    }
    
    public static OperationResult SuccessResult(HttpStatusCode statusCode)
    {
        return new OperationResult { StatusCode = statusCode, Success = true};
    }
    
    public static OperationResult SuccessResult(object data, HttpStatusCode statusCode)
    {
        return new OperationResult(data, statusCode);
    }
    
    public static OperationResult SuccessResult(string message, HttpStatusCode statusCode)
    {
        return new OperationResult(message, statusCode, true);
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