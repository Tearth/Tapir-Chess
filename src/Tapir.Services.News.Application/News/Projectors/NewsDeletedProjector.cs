﻿using Dapper;
using Tapir.Core.Bus;
using Tapir.Core.Persistence;
using Tapir.Services.News.Domain.News.Events;

namespace Tapir.Services.News.Application.News.Projectors
{
    public class NewsDeletedProjector : IEventHandler<NewsDeletedEvent>
    {
        private readonly IDatabaseConnection _database;

        public NewsDeletedProjector(IDatabaseConnection database)
        {
            _database = database;
        }

        public async Task Process(NewsDeletedEvent @event)
        {
            using (var connection = await _database.Open())
            {
                var table = $"News{(@event.IsReplay() ? "_Rebuild" : "")}";

                await connection.ExecuteAsync($"UPDATE {table} SET Deleted = true, DeletedAt = @DeletedAt WHERE Id = @AggregateId", new
                {
                    @event.DeletedAt,
                    @event.AggregateId,
                });
            }
        }
    }
}
