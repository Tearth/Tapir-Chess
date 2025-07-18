﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tapir.Core.Bus;
using Tapir.Core.Events;
using Tapir.Services.Games.Application.Admin.Commands;
using Tapir.Services.Games.Application.Games.Commands;
using Tapir.Services.Games.Application.Games.Projectors;
using Tapir.Services.Games.Application.Games.Queries;
using Tapir.Services.Games.Application.Rooms.Commands;
using Tapir.Services.Games.Application.Rooms.Projectors;
using Tapir.Services.Games.Application.Rooms.Queries;
using Tapir.Services.Games.Application.Tasks;
using Tapir.Services.Games.Domain;
using Tapir.Services.Games.Domain.Rooms.Events;

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
            services.AddScoped<ICreateGameCommandHandler, CreateGameCommandHandler>();
            services.AddScoped<ICancelRoomCommandHandler, CancelRoomCommandHandler>();
            services.AddScoped<ICloseRoomCommandHandler, CloseRoomCommandHandler>();
            services.AddScoped<ICreateRoomCommandHandler, CreateRoomCommandHandler>();
            services.AddScoped<IMakeMoveCommandHandler, MakeMoveCommandHandler>();
            services.AddScoped<IRebuildReadModelCommandHandler, RebuildReadModelCommandHandler>();

            // Query handlers
            services.AddScoped<IGetGameLiveQueryHandler, GetGameLiveHandler>();
            services.AddScoped<IGetRoomListQueryHandler, GetRoomListQueryHandler>();
            services.AddScoped<IGetRoomQueryHandler, GetRoomQueryHandler>();

            // Event handlers
            services.AddScoped<IEventHandler<GameCreatedEvent>, GameCreatedProjector>();
            services.AddScoped<IEventHandler<GameStartedEvent>, GameStartedProjector>();
            services.AddScoped<IEventHandler<MoveMadeEvent>, MoveMadeProjector>();
            services.AddScoped<IEventHandler<RoomCancelledEvent>, RoomCancelledProjector>();
            services.AddScoped<IEventHandler<RoomClosedEvent>, RoomClosedProjector>();
            services.AddScoped<IEventHandler<RoomCreatedEvent>, RoomCreatedProjector>();

            // Tasks
            services.AddScoped<MatchmakingTask>();
            services.AddScoped<ReadModelRebuildingTask>();
            services.AddScoped<ReadModelSynchronizationTask>();

            return services;
        }
    }
}
