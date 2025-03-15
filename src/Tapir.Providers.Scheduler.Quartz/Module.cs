using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.AspNetCore;
using Tapir.Core.Scheduler;

namespace Tapir.Providers.Scheduler.Quartz
{
    public static class Module
    {
        public static IServiceCollection AddQuartzScheduler(this IServiceCollection services, Action<Configuration> userConfiguration)
        {
            var configuration = new Configuration();
            userConfiguration(configuration);

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
            services.AddTransient<ITaskScheduler, Tasks.TaskScheduler>();

            return services;
        }
    }
}
