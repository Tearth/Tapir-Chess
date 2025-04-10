using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.AspNetCore;
using System;
using Tapir.Core.Scheduler;
using Tapir.Providers.Scheduler.Quartz.Tasks;

namespace Tapir.Providers.Scheduler.Quartz
{
    public static class Module
    {
        public static IServiceCollection AddQuartzScheduler(this IServiceCollection services, Action<Configuration> userConfiguration)
        {
            var configuration = new Configuration();
            userConfiguration(configuration);

            // Services
            services.AddScoped<ITaskScheduler, TaskScheduler>();

            // Task scheduler
            services.AddQuartz(configurator =>
            {
                if (string.IsNullOrEmpty(configuration.ConnectionString))
                {
                    throw new InvalidOperationException("Connection string not found.");
                }

                configurator.UsePersistentStore(options =>
                {
                    options.UsePostgres(configuration.ConnectionString);
                    options.UseNewtonsoftJsonSerializer();
                    options.UseClustering();
                });

                configurator.SchedulerId = "AUTO";
            });
            services.AddQuartzServer();

            return services;
        }
    }
}
