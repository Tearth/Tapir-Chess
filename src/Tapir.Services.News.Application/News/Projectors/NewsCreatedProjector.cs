using Dapper;
using Tapir.Core.Bus;
using Tapir.Core.Persistence;
using Tapir.Services.News.Domain.News.Events;

namespace Tapir.Services.News.Application.News.Projectors
{
    public class NewsCreatedProjector : IEventHandler<NewsCreatedEvent>
    {
        private readonly IDatabaseConnection _database;

        public NewsCreatedProjector(IDatabaseConnection database)
        {
            _database = database;
        }

        public async Task Process(NewsCreatedEvent @event)
        {
            using (var connection = _database.Open())
            {
                await connection.ExecuteAsync("INSERT INTO News (Id, CreatedAt) VALUES (@AggregateId, @CreatedAt) ON CONFLICT (Id) DO NOTHING", new
                {
                    @event.AggregateId,
                    @event.CreatedAt,
                });
            }
        }
    }
}
