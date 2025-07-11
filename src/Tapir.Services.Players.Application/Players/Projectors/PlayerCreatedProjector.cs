﻿using Dapper;
using Tapir.Core.Bus;
using Tapir.Core.Persistence;
using Tapir.Services.Players.Domain.Players.Events;

namespace Tapir.Services.Players.Application.Players.Projectors
{
    public class PlayerCreatedProjector : IEventHandler<PlayerCreatedEvent>
    {
        private readonly IDatabaseConnection _database;

        public PlayerCreatedProjector(IDatabaseConnection database)
        {
            _database = database;
        }

        public async Task Process(PlayerCreatedEvent @event)
        {
            using (var connection = await _database.Open())
            {
                var table = $"Players{(@event.IsReplay() ? "_Rebuild" : "")}";

                await connection.ExecuteAsync($"INSERT INTO {table} (Id, CreatedAt) VALUES (@AggregateId, @CreatedAt) ON CONFLICT (Id) DO NOTHING", new
                {
                    @event.AggregateId,
                    @event.CreatedAt
                });
            }
        }
    }
}
