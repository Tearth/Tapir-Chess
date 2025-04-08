using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tapir.Core.Bus;
using Tapir.Core.Events;
using Tapir.Services.News.Application.News.Commands;
using Tapir.Services.News.Application.News.Projectors;
using Tapir.Services.News.Application.News.Queries;
using Tapir.Services.News.Application.Tasks;
using Tapir.Services.News.Domain;
using Tapir.Services.News.Domain.News.Events;

namespace Tapir.Services.News.Application
{

    public static class Module
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHostedService<Startup>();
            services.AddDomain();
            services.AddTransient<IDomainEventSynchronizer, DomainEventSynchronizer>();
            services.AddTransient<IEventBus, EventBus>();
            services.AddTransient<SynchronizeDomainEventsTask>();

            services.AddTransient<ICreateNewsCommandHandler, CreateNewsCommandHandler>();
            services.AddTransient<IDeleteNewsCommandHandler, DeleteNewsCommandHandler>();
            services.AddTransient<IUpdateNewsCommandHandler, UpdateNewsCommandHandler>();

            services.AddTransient<IGetNewsListQueryHandler, GetNewsListQueryHandler>();
            services.AddTransient<IGetNewsQueryHandler, GetNewsQueryHandler>();

            services.AddTransient<IEventHandler<NewsAliasUpdatedEvent>, NewsAliasUpdatedProjector>();
            services.AddTransient<IEventHandler<NewsContentUpdatedEvent>, NewsContentUpdatedProjector>();
            services.AddTransient<IEventHandler<NewsCreatedEvent>, NewsCreatedProjector>();
            services.AddTransient<IEventHandler<NewsDeletedEvent>, NewsDeletedProjector>();
            services.AddTransient<IEventHandler<NewsTitleUpdatedEvent>, NewsTitleUpdatedProjector>();

            return services;
        }
    }
}
