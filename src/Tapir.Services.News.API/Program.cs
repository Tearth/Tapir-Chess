using Tapir.Services.News.Application;
using Tapir.Services.News.Infrastructure;
using Tapir.Providers.EventStore.MongoDB;
using Tapir.Providers.Database.PostgreSQL;

namespace Tapir.Services.News.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();
            builder.Services.AddApplication();
            builder.Services.AddInfrastructure();
            builder.Services.Configure<AppSettings>(builder.Configuration);
           
            var settings = builder.Configuration.Get<AppSettings>();

            if (settings == null)
            {
                throw new InvalidOperationException("AppSettings not found.");
            }

            builder.Services.AddPostgreSqlDatabase(cfg =>
            {
                var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidOperationException("Connection string not found.");
                }

                cfg.ConnectionString = connectionString;
                cfg.MigrationsAssembly = typeof(Infrastructure.Module).Assembly;
            });

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
            app.UsePostgreSqlMigrations();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
