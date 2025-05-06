using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tapir.Providers.EventStore.MongoDB;
using Tapir.Providers.Database.PostgreSQL;
using System.Reflection;
using Tapir.Providers.Scheduler.Quartz;
using Serilog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Tapir.Services.Games.Infrastructure
{
    public static class Module
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = configuration.Get<AppSettings>() ?? throw new InvalidOperationException("AppSettings not found.");

            // Initialization
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

            // Logger
            services.AddSerilog((context, cfg) =>
            {
                cfg.ReadFrom.Configuration(configuration);
            });

            return services;
        }
    }
}
