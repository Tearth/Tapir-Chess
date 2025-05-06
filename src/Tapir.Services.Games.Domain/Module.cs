using Microsoft.Extensions.DependencyInjection;
using Tapir.Core.Events;
using Tapir.Core.Persistence;

namespace Tapir.Services.Games.Domain
{
    public static class Module
    {
        public static IServiceCollection AddDomain(this IServiceCollection services)
        {
            var eventRegistry = new DomainEventRegistry();

            // Services
            services.AddSingleton<IDomainEventRegistry>(eventRegistry);

            // Repositories

            // Event handlers

            return services;
        }
    }
}
