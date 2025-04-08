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

        public async Task Process(NewsContentUpdatedEvent notification)
        {
            using (var connection = _database.Open())
            {
                await connection.ExecuteAsync("UPDATE News SET Content = @Content, UpdatedAt = NOW() WHERE Id = @AggregateId", new
                {
                    notification.Content,
                    notification.AggregateId,
                });
            }
        }
    }
}
