using Dapper;
using Tapir.Core.Bus;
using Tapir.Core.Persistence;
using Tapir.Services.News.Domain.News.Events;

namespace Tapir.Services.News.Application.News.Projectors
{
    public class NewsAliasUpdatedProjector : IEventHandler<NewsAliasUpdatedEvent>
    {
        private readonly IDatabaseConnection _database;

        public NewsAliasUpdatedProjector(IDatabaseConnection database)
        {
            _database = database;
        }

        public async Task Process(NewsAliasUpdatedEvent @event)
        {
            using (var connection = await _database.Open())
            {
                var table = $"News{(@event.IsReplay() ? "_Rebuild" : "")}";

                await connection.ExecuteAsync($"UPDATE {table} SET Alias = @Alias, UpdatedAt = NOW() WHERE Id = @AggregateId", new
                {
                    @event.Alias,
                    @event.AggregateId,
                });
            }
        }
    }
}