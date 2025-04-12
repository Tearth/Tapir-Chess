using Microsoft.Extensions.DependencyInjection;
using Tapir.Core.Events;
using Tapir.Core.Persistence;
using Tapir.Services.Players.Domain.Players.Entities;
using Tapir.Services.Players.Domain.Players.Events;

namespace Tapir.Services.Players.Domain
{
    public static class Module
    {
        public static IServiceCollection AddDomain(this IServiceCollection services)
        {
            var eventRegistry = new DomainEventRegistry();
            
            // Services
            services.AddSingleton<IDomainEventRegistry>(eventRegistry);

            // Repositories
            services.AddScoped<IAggregateRepository<PlayerEntity>, AggregateRepository<PlayerEntity>>();

            // Event handlers
            eventRegistry.Add<PlayerAboutMeUpdatedEvent>();
            eventRegistry.Add<PlayerCountryUpdatedEvent>();
            eventRegistry.Add<PlayerCreatedEvent>();
            eventRegistry.Add<PlayerEmailUpdatedEvent>();
            eventRegistry.Add<PlayerUsernameUpdatedEvent>();

            return services;
        }
    }
}
