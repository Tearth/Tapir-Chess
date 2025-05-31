using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tapir.Core.Bus;
using Tapir.Core.Events;
using Tapir.Core.Messaging.Identity;
using Tapir.Services.News.Application.News.Commands;
using Tapir.Services.Players.Application.Admin.Commands;
using Tapir.Services.Players.Application.Messages;
using Tapir.Services.Players.Application.Players.Commands;
using Tapir.Services.Players.Application.Players.Projectors;
using Tapir.Services.Players.Application.Players.Queries;
using Tapir.Services.Players.Application.Tasks;
using Tapir.Services.Players.Domain;
using Tapir.Services.Players.Domain.Players.Events;

namespace Tapir.Services.Players.Application
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

            // Command handlers
            services.AddScoped<ICreatePlayerCommandHandler, CreatePlayerCommandHandler>();
            services.AddScoped<IRebuildReadModelCommandHandler, RebuildReadModelCommandHandler>();
            services.AddScoped<IUpdatePlayerIdentityCommandHandler, UpdatePlayerIdentityCommandHandler>();
            services.AddScoped<IUpdatePlayerProfileCommandHandler, UpdatePlayerProfileCommandHandler>();

            // Query handlers
            services.AddScoped<IGetPlayerListQueryHandler, GetPlayerListQueryHandler>();
            services.AddScoped<IGetPlayerQueryHandler, GetPlayerQueryHandler>();

            // Event handlers
            services.AddScoped<IEventHandler<PlayerAboutMeUpdatedEvent>, PlayerAboutMeUpdatedProjector>();
            services.AddScoped<IEventHandler<PlayerCountryUpdatedEvent>, PlayerCountryUpdatedProjector>();
            services.AddScoped<IEventHandler<PlayerCreatedEvent>, PlayerCreatedProjector>();
            services.AddScoped<IEventHandler<PlayerEmailUpdatedEvent>, PlayerEmailUpdatedProjector>();
            services.AddScoped<IEventHandler<PlayerUsernameUpdatedEvent>, PlayerUsernameUpdatedProjector>();

            // Message handlers
            services.AddScoped<IEventHandler<UserCreatedMessage>, UserCreatedMessageHandler>();
            services.AddScoped<IEventHandler<UserUpdatedMessage>, UserUpdatedMessageHandler>();

            // Tasks
            services.AddScoped<ReadModelRebuildingTask>();
            services.AddScoped<ReadModelSynchronizationTask>();

            return services;
        }
    }
}
