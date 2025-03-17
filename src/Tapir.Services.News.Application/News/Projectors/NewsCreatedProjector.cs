using Dapper;
using MediatR;
using Tapir.Core.Persistence;
using Tapir.Services.News.Domain.News.Events;

namespace Tapir.Services.News.Application.News.Projectors
{
    public class NewsCreatedProjector : INotificationHandler<NewsCreatedEvent>
    {
        private readonly IDatabaseConnection _database;

        public NewsCreatedProjector(IDatabaseConnection database)
        {
            _database = database;
        }

        public async Task Handle(NewsCreatedEvent notification, CancellationToken cancellationToken)
        {
            using (var connection = _database.Open())
            {
                await connection.ExecuteAsync("INSERT INTO News (Id, CreatedAt) VALUES (@AggregateId, @CreatedAt)", new
                {
                    notification.AggregateId,
                    notification.CreatedAt,
                });
            }
        }
    }
}
