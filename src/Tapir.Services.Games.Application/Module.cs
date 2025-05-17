using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tapir.Core.Bus;
using Tapir.Core.Events;
using Tapir.Services.Games.Application.Rooms.Commands;
using Tapir.Services.Games.Application.Rooms.Projectors;
using Tapir.Services.Games.Application.Rooms.Queries;
using Tapir.Services.Games.Domain;
using Tapir.Services.Games.Domain.Rooms.Events;
using Tapir.Services.Rooms.Application.Tasks;

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
            services.AddScoped<ICancelRoomCommandHandler, CancelRoomCommandHandler>();
            services.AddScoped<ICloseRoomCommandHandler, CloseRoomCommandHandler>();
            services.AddScoped<ICreateRoomCommandHandler, CreateRoomCommandHandler>();

            // Query handlers
            services.AddScoped<IGetRoomListQueryHandler, GetRoomListQueryHandler>();
            services.AddScoped<IGetRoomQueryHandler, GetRoomQueryHandler>();

            // Event handlers
            services.AddScoped<IEventHandler<RoomCancelledEvent>, RoomCancelledProjector>();
            services.AddScoped<IEventHandler<RoomClosedEvent>, RoomClosedProjector>();
            services.AddScoped<IEventHandler<RoomCreatedEvent>, RoomCreatedProjector>();

            // Tasks
            services.AddScoped<DomainEventSynchronizationTask>();

            return services;
        }
    }
}
