using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tapir.Providers.EventStore.MongoDB;
using Tapir.Providers.Database.PostgreSQL;
using System.Reflection;
using Tapir.Providers.Scheduler.Quartz;
using Serilog;
using Tapir.Core.Events;

namespace Tapir.Services.Players.Infrastructure
{
    public static class Module
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = configuration.Get<AppSettings>();
            if (settings == null)
            {
                throw new InvalidOperationException("AppSettings not found.");
            }

            services.AddPostgreSqlDatabase(cfg =>
            {
                var connectionString = configuration.GetConnectionString("DefaultConnection");

                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidOperationException("Connection string not found.");
                }

                cfg.ConnectionString = connectionString;
                cfg.MigrationsAssembly = Assembly.GetExecutingAssembly();
            });

            services.AddMongoDBEventStore(cfg =>
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

            services.AddQuartzScheduler(cfg =>
            {
                var connectionString = configuration.GetConnectionString("DefaultConnection");

                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidOperationException("Connection string not found.");
                }

                cfg.ConnectionString = connectionString;
            });

            services.AddSerilog((context, cfg) =>
            {
                cfg.ReadFrom.Configuration(configuration);
            });

            services.AddTransient<IDomainEventBus, DomainEventBus>();

            return services;
        }
    }
}
