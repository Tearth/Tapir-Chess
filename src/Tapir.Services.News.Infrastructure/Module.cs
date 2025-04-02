using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tapir.Providers.EventStore.MongoDB;
using Tapir.Providers.Database.PostgreSQL;
using System.Reflection;
using Tapir.Providers.Scheduler.Quartz;
using Serilog;
using MediatR;
using Tapir.Core.Pipeline;

namespace Tapir.Services.News.Infrastructure
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

            services.AddMongoDbEventStore(cfg =>
            {
                if (settings.Database == null)
                {
                    throw new InvalidOperationException("MongoDb settings not found.");
                }

                if (settings.Database.Servers == null || settings.Database.Servers.Count == 0)
                {
                    throw new InvalidOperationException("MongoDb server settings not found.");
                }

                cfg.Servers = settings.Database.Servers.Select(p => new ServerConfiguration
                {
                    Host = p.Host,
                    Port = p.Port
                }).ToList();

                cfg.DatabaseName = settings.Database.DatabaseName;
                cfg.AuthenticationMethod = settings.Database.AuthenticationMethod;
                cfg.Username = settings.Database.Username;
                cfg.Password = settings.Database.Password;
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

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RetryBehavior<,>));

            return services;
        }
    }
}
