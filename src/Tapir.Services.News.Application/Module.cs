using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tapir.Core.Bus;
using Tapir.Core.Events;
using Tapir.Services.News.Application.Admin.Commands;
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
            // Initialization
            services.AddHostedService<Startup>();
            services.AddDomain();

            // Services
            services.AddScoped<IDomainEventSynchronizer, DomainEventSynchronizer>();
            services.AddSingleton<IEventBus, EventBus>();

            // Command handlers
            services.AddScoped<ICreateNewsCommandHandler, CreateNewsCommandHandler>();
            services.AddScoped<IDeleteNewsCommandHandler, DeleteNewsCommandHandler>();
            services.AddScoped<IRebuildReadModelCommandHandler, RebuildReadModelCommandHandler>();
            services.AddScoped<IUpdateNewsCommandHandler, UpdateNewsCommandHandler>();

            // Query handlers
            services.AddScoped<IGetNewsListQueryHandler, GetNewsListQueryHandler>();
            services.AddScoped<IGetNewsQueryHandler, GetNewsQueryHandler>();

            // Event handlers
            services.AddScoped<IEventHandler<NewsAliasUpdatedEvent>, NewsAliasUpdatedProjector>();
            services.AddScoped<IEventHandler<NewsContentUpdatedEvent>, NewsContentUpdatedProjector>();
            services.AddScoped<IEventHandler<NewsCreatedEvent>, NewsCreatedProjector>();
            services.AddScoped<IEventHandler<NewsDeletedEvent>, NewsDeletedProjector>();
            services.AddScoped<IEventHandler<NewsTitleUpdatedEvent>, NewsTitleUpdatedProjector>();

            // Tasks
            services.AddScoped<ReadModelRebuildingTask>();
            services.AddScoped<ReadModelSynchronizationTask>();

            return services;
        }
    }
}
