using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Tapir.Core.Domain.Exceptions;
using Tapir.Core.Persistence.Exceptions;

namespace Tapir.Services.Players.API.Middleware
{
    public class ExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<ExceptionHandler> _logger;

        public ExceptionHandler(ILogger<ExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            ProblemDetails? problemDetails = null;

            switch (exception)
            {
                case AggregateNotFoundException aggregateNotFoundException:
                {
                    problemDetails = new ProblemDetails
                    {
                        Title = "Resource with the specified ID does not exists.",
                        Detail = "ResourceNotFound",
                        Status = StatusCodes.Status404NotFound,
                        Type = "https://tools.ietf.org/html/rfc9110#section-15.5.5"
                    };

                    break;
                }
                case DomainException domainException:
                {
                    problemDetails = new ProblemDetails
                    {
                        Title = domainException.Message,
                        Detail = domainException.ErrorCode,
                        Status = StatusCodes.Status400BadRequest,
                        Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1"
                    };

                    break;
                }
                default:
                {
                    problemDetails = new ProblemDetails
                    {
                        Title = "An error occurred while processing your request.",
                        Type = "https://tools.ietf.org/html/rfc9110#section-15.6.1",
                        Status = StatusCodes.Status500InternalServerError,
                    };

                    _logger.LogError(exception, "Internal server error");

                    break;
                }
            }


            problemDetails.Extensions = new Dictionary<string, object?>
            {
                { "trackId", httpContext.TraceIdentifier }
            };
            httpContext.Response.StatusCode = problemDetails.Status!.Value;

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
            return true;
        }
    }
}