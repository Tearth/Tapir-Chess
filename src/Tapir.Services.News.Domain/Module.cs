using Microsoft.Extensions.DependencyInjection;
using Tapir.Core.Events;
using Tapir.Core.Persistence;
using Tapir.Services.News.Domain.News.Entities;
using Tapir.Services.News.Domain.News.Events;

namespace Tapir.Services.News.Domain
{
    public static class Module
    {
        public static IServiceCollection AddDomain(this IServiceCollection services)
        {
            var eventRegistry = new DomainEventRegistry();

            // Services
            services.AddSingleton<IDomainEventRegistry>(eventRegistry);

            // Repositories
            services.AddScoped<IAggregateRepository<NewsEntity>, AggregateRepository<NewsEntity>>();

            // Events
            eventRegistry.Add<NewsAliasUpdatedEvent>();
            eventRegistry.Add<NewsContentUpdatedEvent>();
            eventRegistry.Add<NewsCreatedEvent>();
            eventRegistry.Add<NewsTitleUpdatedEvent>();
            eventRegistry.Add<NewsDeletedEvent>();

            return services;
        }
    }
}
