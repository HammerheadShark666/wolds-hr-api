namespace wolds_hr_api.Library.ExceptionHandlers;

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading;
using wolds_hr_api.Library.Exceptions;

internal sealed class GlobalExceptionHandler(IProblemDetailsService problemDetailsService, ILogger<GlobalExceptionHandler> _logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "An unhandled exception occurred while processing request {Method} {Path}",
                                                                httpContext.Request.Method, httpContext.Request.Path);

        var (statusCode, message) = exception switch
        {
            DepartmentNotFoundException or EmployeeNotFoundException => ((int)HttpStatusCode.NotFound, exception.Message),
            UnauthorizedAccessException => ((int)HttpStatusCode.Unauthorized, "Unauthorized"),
            _ => ((int)HttpStatusCode.InternalServerError, "An unexpected error occurred.")
        };

        httpContext.Response.StatusCode = statusCode;

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails
            {
                Type = exception.GetType().Name,
                Title = "An error occurred",
                Detail = message
            }
        });
    }
}