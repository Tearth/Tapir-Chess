using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Tapir.Services.Games.API.Controllers;
using Tapir.Services.Games.API.Middleware;
using Tapir.Services.Games.Application;
using Tapir.Services.Games.Infrastructure;

namespace Tapir.Services.Games.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();
            builder.Services.AddHttpContextAccessor();
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
            builder.Services.AddSignalR();

            var app = builder.Build();
            app.UseAuthorization();
            app.MapControllers();
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseExceptionHandler();
            app.MapHub<WebSocketController>("/api/games/ws");
            app.Run();
        }
    }
}
