using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;
using Tapir.Core.Bus;
using Tapir.Services.Games.API.Events;
using Tapir.Services.Games.API.Hubs;
using Tapir.Services.Games.API.Middleware;
using Tapir.Services.Games.Application;
using Tapir.Services.Games.Application.Games.Projectors;
using Tapir.Services.Games.Domain.Rooms.Events;
using Tapir.Services.Games.Infrastructure;

namespace Tapir.Services.Games.API
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
            builder.Services.AddSignalR().AddJsonProtocol(options =>
            {
                options.PayloadSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            // Event handlers
            builder.Services.AddScoped<IEventHandler<GameCreatedEvent>, GameCreatedEventHandler>();

            var app = builder.Build();
            app.UseAuthorization();
            app.MapControllers();
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseExceptionHandler();
            app.MapHub<WebSocketHub>("/api/games/ws");
            app.Run();
        }
    }
}
