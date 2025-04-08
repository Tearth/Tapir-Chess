using Dapper;
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

        public async Task Process(NewsDeletedEvent notification)
        {
            using (var connection = _database.Open())
            {
                await connection.ExecuteAsync("UPDATE News SET Deleted = true, DeletedAt = @DeletedAt WHERE Id = @AggregateId", new
                {
                    notification.DeletedAt,
                    notification.AggregateId,
                });
            }
        }
    }
}
