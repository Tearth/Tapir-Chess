﻿using Dapper;
using Tapir.Core.Bus;
using Tapir.Core.Persistence;
using Tapir.Services.Players.Domain.Players.Events;

namespace Tapir.Services.News.Application.News.Projectors
{
    public class PlayerUsernameUpdatedProjector : IEventHandler<PlayerUsernameUpdatedEvent>
    {
        private readonly IDatabaseConnection _database;

        public PlayerUsernameUpdatedProjector(IDatabaseConnection database)
        {
            _database = database;
        }

        public async Task Process(PlayerUsernameUpdatedEvent @event)
        {
            using (var connection = _database.Open())
            {
                await connection.ExecuteAsync("UPDATE Players SET Username = @Username, UpdatedAt = NOW() WHERE Id = @AggregateId", new
                {
                    @event.Username,
                    @event.AggregateId,
                });
            }
        }
    }
}
