using Tapir.Services.News.Application;
using Tapir.Providers.EventStore.MongoDB;

namespace Tapir.Services.News.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();
            builder.Services.AddApplication();
            builder.Services.Configure<AppSettings>(builder.Configuration);
           
            var settings = builder.Configuration.Get<AppSettings>();

            if (settings == null)
            {
                throw new InvalidOperationException("AppSettings not found.");
            }

            builder.Services.AddMongoDBEventStore(cfg =>
            {
                if (settings.MongoDb == null)
                {
                    throw new InvalidOperationException("MongoDb settings not found.");
                }

                if (settings.MongoDb.Servers == null || settings.MongoDb.Servers.Count == 0)
                {
                    throw new InvalidOperationException("MongoDb server settings not found.");
                }

                cfg.Servers = settings.MongoDb.Servers.Select(p => new ServerConfiguration
                {
                    Host = p.Host,
                    Port = p.Port
                }).ToList();
                cfg.DatabaseName = settings.MongoDb.DatabaseName;

                cfg.AuthenticationMethod = settings.MongoDb.AuthenticationMethod;
                cfg.Username = settings.MongoDb.Username;
                cfg.Password = settings.MongoDb.Password;
            });

            var app = builder.Build();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
