using Microsoft.OpenApi.Models;
using Tapir.Services.News.Application;
using Tapir.Services.News.Infrastructure;

namespace Tapir.Services.News.API
{
    public class Program
    {
        public static async Task Main(string[] args)
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

            var app = builder.Build();
            await app.UseApplication();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.UseSwagger();
            app.UseSwaggerUI();
            app.Run();
        }
    }
}
