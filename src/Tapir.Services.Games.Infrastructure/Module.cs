using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tapir.Providers.EventStore.MongoDB;
using Tapir.Providers.Database.PostgreSQL;
using System.Reflection;
using Tapir.Providers.Scheduler.Quartz;
using Serilog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Tapir.Providers.MessageBus.RabbitMQ;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Tapir.Providers.Chess.PaxChess;

namespace Tapir.Services.Games.Infrastructure
{
    public static class Module
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = configuration.Get<AppSettings>() ?? throw new InvalidOperationException("AppSettings not found.");

            // Initialization
            services.AddHostedService<Startup>();
            services.AddSingleton(settings);

            // Authentication
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    if (settings.Jwt == null)
                    {
                        throw new InvalidOperationException("JWT settings not found.");
                    }

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = settings.Jwt.Issuer,
                        ValidAudience = settings.Jwt.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Jwt.Secret))
                    };
                });

            // Database
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

            // Event store
            services.AddMongoDbEventStore(cfg =>
            {
                if (settings.EventStore == null)
                {
                    throw new InvalidOperationException("Event store settings not found.");
                }

                if (settings.EventStore.Servers == null || settings.EventStore.Servers.Count == 0)
                {
                    throw new InvalidOperationException("Event store server settings not found.");
                }

                cfg.Servers = settings.EventStore.Servers.Select(p => new ServerConfiguration
                {
                    Host = p.Host,
                    Port = p.Port
                }).ToList();

                cfg.DatabaseName = settings.EventStore.DatabaseName;
                cfg.AuthenticationMethod = settings.EventStore.AuthenticationMethod;
                cfg.Username = settings.EventStore.Username;
                cfg.Password = settings.EventStore.Password;
            });

            // Task scheduler
            services.AddQuartzScheduler(cfg =>
            {
                var connectionString = configuration.GetConnectionString("DefaultConnection");

                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidOperationException("Connection string not found.");
                }

                cfg.ConnectionString = connectionString;
            });

            // Message bus
            services.AddRabbitMqMessageBus(cfg =>
            {
                cfg.Host = settings.MessageBus.Host;
                cfg.Port = settings.MessageBus.Port;
                cfg.Username = settings.MessageBus.Username;
                cfg.Password = settings.MessageBus.Password;
                cfg.QueueName = settings.MessageBus.QueueName;
            });

            // Chess
            services.AddPaxChess();

            // Logger
            services.AddSerilog((context, cfg) =>
            {
                cfg.ReadFrom.Configuration(configuration);
            });

            return services;
        }
    }
}
