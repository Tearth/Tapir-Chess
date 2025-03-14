using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tapir.Core.Events;
using Tapir.Core.Scheduler;
using Tapir.Services.News.Application.Tasks;
using Tapir.Services.News.Domain;

namespace Tapir.Services.News.Application
{
    public static class Module
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDomain();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Module).Assembly));
            services.AddTransient<IDomainEventSynchronizer, DomainEventSynchronizer>();
            services.AddTransient<SynchronizeDomainEventsTask>();

            return services;
        }

        public static async Task<IApplicationBuilder> UseApplication(this IApplicationBuilder app)
        {
            var scheduler = app.ApplicationServices.GetService<ITaskScheduler>();

            await scheduler.Register(new SynchronizeDomainEventsTask(), "0/1 * * * * ?");

            return app;
        }
    }
}
