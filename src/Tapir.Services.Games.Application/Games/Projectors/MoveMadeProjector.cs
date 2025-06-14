﻿using Dapper;
using Tapir.Core.Bus;
using Tapir.Core.Persistence;
using Tapir.Services.Games.Domain.Rooms.Events;

namespace Tapir.Services.Games.Application.Games.Projectors
{
    public class MoveMadeProjector : IEventHandler<MoveMadeEvent>
    {
        private readonly IDatabaseConnection _database;

        public MoveMadeProjector(IDatabaseConnection database)
        {
            _database = database;
        }

        public async Task Process(MoveMadeEvent @event)
        {
            using (var connection = await _database.Open())
            {
                var table = $"Games{(@event.IsReplay() ? "_Rebuild" : "")}";

                await connection.ExecuteAsync($"UPDATE {table} SET TimeWhite = @TimeWhite, TimeBlack = @TimeBlack, MovesNumber = @MoveNumber, Fen = @Fen, UpdatedAt = NOW() WHERE Id = @AggregateId", new
                {
                    @event.TimeWhite,
                    @event.TimeBlack,
                    @event.MoveNumber,
                    @event.Fen,
                    @event.AggregateId,
                });
            }
        }
    }
}
