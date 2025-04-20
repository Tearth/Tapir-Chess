using Microsoft.AspNetCore.Mvc;

namespace Tapir.Identity.API.Middleware
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

            problemDetails.Errors = problemDetails.Errors.ToDictionary(
                pair => char.ToLower(pair.Key[0]) + pair.Key[1..],
                pair => pair.Value
            );

            return new BadRequestObjectResult(problemDetails);
        }
    }
}
