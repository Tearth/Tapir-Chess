using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tapir.Core.Bus;
using Tapir.Core.Events;
using Tapir.Services.Games.Domain;

namespace Tapir.Services.Games.Application
{

    public static class Module
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            // Initialization
            services.AddHostedService<Startup>();
            services.AddDomain();

            // Services
            services.AddScoped<IDomainEventSynchronizer, DomainEventSynchronizer>();
            services.AddSingleton<IEventBus, EventBus>();

            // Command handlers

            // Query handlers

            // Event handlers

            // Tasks

            return services;
        }
    }
}
