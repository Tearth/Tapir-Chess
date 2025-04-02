using Microsoft.Extensions.DependencyInjection;
using Tapir.Core.Events;
using Tapir.Core.Persistence;
using Tapir.Services.Players.Domain.Players.Entities;

namespace Tapir.Services.Players.Domain
{
    public static class Module
    {
        public static IServiceCollection AddDomain(this IServiceCollection services)
        {
            var eventRegistry = new DomainEventRegistry();

            services.AddSingleton<IDomainEventRegistry>(eventRegistry);
            services.AddTransient<IAggregateRepository<PlayerEntity>, AggregateRepository<PlayerEntity>>();

            return services;
        }
    }
}
