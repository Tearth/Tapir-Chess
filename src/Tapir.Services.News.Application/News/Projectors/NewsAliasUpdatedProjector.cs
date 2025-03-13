using Dapper;
using MediatR;
using Tapir.Core.Persistence;
using Tapir.Services.News.Domain.News.Events;

namespace Tapir.Services.News.Application.News.Projectors
{
    public class NewsAliasUpdatedProjector : INotificationHandler<NewsAliasUpdatedEvent>
    {
        private readonly IDatabaseConnection _database;

        public NewsAliasUpdatedProjector(IDatabaseConnection database)
        {
            _database = database;
        }

        public async Task Handle(NewsAliasUpdatedEvent notification, CancellationToken cancellationToken)
        {
            using (var connection = _database.Open())
            {
                await connection.ExecuteAsync("UPDATE News SET Alias = @Alias, UpdatedAt = NOW() WHERE AggregateId = @AggregateId", new
                {
                    notification.Alias,
                    notification.AggregateId,
                });
            }
        }
    }
}
