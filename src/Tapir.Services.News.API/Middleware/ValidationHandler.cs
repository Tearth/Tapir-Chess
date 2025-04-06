using Microsoft.AspNetCore.Mvc;

namespace Tapir.Services.News.API.Middleware
{
    public static class ValidationHandler
    {
        public static IActionResult InvalidModelStateResponseFactory(ApiBehaviorOptions options, ActionContext actionContext)
        {
            var problemDetails = new ValidationProblemDetails(actionContext.ModelState);

            problemDetails.Type = options.ClientErrorMapping[400].Link;
            problemDetails.Title = "One or more validation errors occurred.";
            problemDetails.Status = StatusCodes.Status400BadRequest;
            problemDetails.Extensions = new Dictionary<string, object?>
            {
                { "trackId", actionContext.HttpContext.TraceIdentifier }
            };

            return new BadRequestObjectResult(problemDetails);
        }
    }
}
