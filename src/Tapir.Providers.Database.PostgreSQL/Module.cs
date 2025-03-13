using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Tapir.Core.Persistence;
using Tapir.Providers.Database.PostgreSQL.Persistence;

namespace Tapir.Providers.Database.PostgreSQL
{
    public static class Module
    {
        public static IServiceCollection AddPostgreSqlDatabase(this IServiceCollection services, Action<Configuration> userConfiguration)
        {
            var configuration = new Configuration();
            userConfiguration(configuration);

            if (string.IsNullOrEmpty(configuration.ConnectionString))
            {
                throw new InvalidOperationException("Connection string not found.");
            }

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
