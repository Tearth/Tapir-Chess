using Dapper;
using MediatR;
using Tapir.Core.Persistence;
using Tapir.Services.News.Domain.News.Events;

namespace Tapir.Services.News.Application.News.Projectors
{
    public class NewsDeletedProjector : INotificationHandler<NewsDeletedEvent>
    {
        private readonly IDatabaseConnection _database;

        public NewsDeletedProjector(IDatabaseConnection database)
        {
            _database = database;
        }

        public async Task Handle(NewsDeletedEvent notification, CancellationToken cancellationToken)
        {
            using (var connection = _database.Open())
            {
                await connection.ExecuteAsync("UPDATE News SET Deleted = 1, DeletedAt = @DeletedAt WHERE Id = @AggregateId", new
                {
                    notification.DeletedAt,
                    notification.AggregateId,
                });
            }
        }
    }
}
