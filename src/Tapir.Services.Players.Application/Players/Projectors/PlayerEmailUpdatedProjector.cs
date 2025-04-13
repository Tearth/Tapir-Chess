﻿using Dapper;
using Tapir.Core.Bus;
using Tapir.Core.Persistence;
using Tapir.Services.Players.Domain.Players.Events;

namespace Tapir.Services.Players.Application.Players.Projectors
{
    public class PlayerEmailUpdatedProjector : IEventHandler<PlayerEmailUpdatedEvent>
    {
        private readonly IDatabaseConnection _database;

        public PlayerEmailUpdatedProjector(IDatabaseConnection database)
        {
            _database = database;
        }

        public async Task Process(PlayerEmailUpdatedEvent @event)
        {
            using (var connection = _database.Open())
            {
                await connection.ExecuteAsync("UPDATE Players SET Email = @Email, UpdatedAt = NOW() WHERE Id = @AggregateId", new
                {
                    @event.Email,
                    @event.AggregateId,
                });
            }
        }
    }
}
