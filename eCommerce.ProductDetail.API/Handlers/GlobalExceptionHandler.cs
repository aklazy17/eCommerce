using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.ProductDetail.API.Handlers;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger = logger;

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "An unhandled exception has occurred.");

        (string title, int statusCode) = GetStatusCode(exception);

        var problemDetails = new ProblemDetails
        {
            Title = title,
            Status = statusCode,
            Detail = exception.Message
        };

        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }

    private Tuple<string, int> GetStatusCode(Exception exception)
    {
        if (exception is BadHttpRequestException ||
            exception is InvalidOperationException)
        {
            return new Tuple<string, int>("Invalid Operation", StatusCodes.Status400BadRequest);
        }
        else if (exception is KeyNotFoundException)
        {
            return new Tuple<string, int>("Not Found", StatusCodes.Status404NotFound);
        }
        else
        {
            return new Tuple<string, int>("An error occurred", StatusCodes.Status502BadGateway);
        }
    }
}