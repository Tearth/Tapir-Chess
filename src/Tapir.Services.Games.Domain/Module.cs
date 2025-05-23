﻿using Microsoft.Extensions.DependencyInjection;
using Tapir.Core.Events;
using Tapir.Core.Persistence;
using Tapir.Services.Games.Domain.Rooms.Entities;
using Tapir.Services.Games.Domain.Rooms.Events;

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
            services.AddScoped<IAggregateRepository<GameEntity>, AggregateRepository<GameEntity>>();
            services.AddScoped<IAggregateRepository<RoomEntity>, AggregateRepository<RoomEntity>>();

            // Events
            eventRegistry.Add<GameCreatedEvent>();
            eventRegistry.Add<GameStartedEvent>();
            eventRegistry.Add<MoveMadeEvent>();
            eventRegistry.Add<RoomCancelledEvent>();
            eventRegistry.Add<RoomClosedEvent>();
            eventRegistry.Add<RoomCreatedEvent>();

            return services;
        }
    }
}
