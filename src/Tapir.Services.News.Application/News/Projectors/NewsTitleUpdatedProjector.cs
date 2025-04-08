using Dapper;
using Tapir.Core.Bus;
using Tapir.Core.Persistence;
using Tapir.Services.News.Domain.News.Events;

namespace Tapir.Services.News.Application.News.Projectors
{
    public class NewsTitleUpdatedProjector : IEventHandler<NewsTitleUpdatedEvent>
    {
        private readonly IDatabaseConnection _database;

        public NewsTitleUpdatedProjector(IDatabaseConnection database)
        {
            _database = database;
        }

        public async Task Process(NewsTitleUpdatedEvent notification)
        {
            using (var connection = _database.Open())
            {
                await connection.ExecuteAsync("UPDATE News SET Title = @Title, UpdatedAt = NOW() WHERE Id = @AggregateId", new
                {
                    notification.Title,
                    notification.AggregateId,
                });
            }
        }
    }
}
