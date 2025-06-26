using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Metrics;
using Tapir.Identity.API.Middleware;
using Tapir.Identity.Application;
using Tapir.Identity.Infrastructure;

namespace Tapir.Identity.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();
            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddApplication(builder.Configuration);
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
            });
            builder.Services.AddExceptionHandler<ExceptionHandler>();
            builder.Services.AddProblemDetails();
            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (actionContext) => ValidationHandler.InvalidModelStateResponseFactory(options, actionContext);
            });

            // OpenTelemetry
            builder.Services.AddOpenTelemetry().WithMetrics(cfg =>
            {
                cfg.AddPrometheusExporter();
                cfg.AddMeter("Microsoft.AspNetCore.Hosting", "Microsoft.AspNetCore.Server.Kestrel");
                cfg.AddView("http.server.request.duration", new ExplicitBucketHistogramConfiguration
                {
                    Boundaries = [0, 0.005, 0.01, 0.025, 0.05, 0.075, 0.1, 0.25, 0.5, 0.75, 1, 2.5, 5, 7.5, 10]
                });
            });

            var app = builder.Build();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseExceptionHandler();
            app.MapPrometheusScrapingEndpoint();
            app.Run();
        }
    }
}
