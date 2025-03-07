using Microsoft.Extensions.DependencyInjection;
using Tapir.Core.Domain;
using Tapir.Core.Interfaces;
using Tapir.Services.News.Domain.News.Events;

namespace Tapir.Services.News.Domain
{
    public static class Module
    {
        public static IServiceCollection AddDomain(this IServiceCollection services)
        {
            var eventRegistry = new DomainEventRegistry();

            eventRegistry.Add<NewsAliasUpdatedEvent>();
            eventRegistry.Add<NewsContentUpdatedEvent>();
            eventRegistry.Add<NewsCreatedEvent>();
            eventRegistry.Add<NewsTitleUpdatedEvent>();

            services.AddSingleton<IDomainEventRegistry>(eventRegistry);

            return services;
        }
    }
}
