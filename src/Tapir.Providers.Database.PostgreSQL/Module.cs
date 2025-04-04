﻿using Microsoft.Extensions.DependencyInjection;
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

            services.AddHostedService<Startup>();

            services.AddTransient<IDatabaseConnection, DatabaseConnection>(p => new DatabaseConnection(
                configuration.ConnectionString    
            ));

            services.AddSingleton(configuration);

            return services;
        }
    }
}
