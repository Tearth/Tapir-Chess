using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Tapir.Core.Interfaces;
using Tapir.Providers.Database.PostgreSQL.Implementations;

namespace Tapir.Providers.Database.PostgreSQL
{
    public static class Module
    {
        public static IServiceCollection AddPostgreSqlDatabase(this IServiceCollection services, Action<Configuration> userConfiguration)
        {
            var configuration = new Configuration();
            userConfiguration(configuration);

            services.AddTransient<IDatabaseConnection, DatabaseConnection>(p => new DatabaseConnection(
                configuration.ConnectionString    
            ));
            services.AddSingleton(configuration);

            return services;
        }

        public static IApplicationBuilder UsePostgreSqlMigrations(this IApplicationBuilder app)
        {
            var configuration = app.ApplicationServices.GetService<Configuration>();
            if (configuration == null)
            {
                throw new InvalidOperationException("Database connection not found.");
            }

            new DatabaseMigrations(configuration).Execute();

            return app;
        }
    }
}
