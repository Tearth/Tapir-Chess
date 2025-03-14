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
            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddApplication(builder.Configuration);
            
            var app = builder.Build();
            await app.UseInfrastructure();
            await app.UseApplication();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
