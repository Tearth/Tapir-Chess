using Dapper;
using Tapir.Core.Bus;
using Tapir.Core.Persistence;
using Tapir.Services.News.Domain.News.Events;

namespace Tapir.Services.News.Application.News.Projectors
{
    public class NewsContentUpdatedProjector : IEventHandler<NewsContentUpdatedEvent>
    {
        private readonly IDatabaseConnection _database;

        public NewsContentUpdatedProjector(IDatabaseConnection database)
        {
            _database = database;
        }

        public async Task Process(NewsContentUpdatedEvent @event)
        {
            using (var connection = await _database.Open())
            {
                var table = $"News{(@event.IsReplay() ? "_Rebuild" : "")}";

                await connection.ExecuteAsync($"UPDATE {table} SET Content = @Content, UpdatedAt = NOW() WHERE Id = @AggregateId", new
                {
                    @event.Content,
                    @event.AggregateId,
                });
            }
        }
    }
}
