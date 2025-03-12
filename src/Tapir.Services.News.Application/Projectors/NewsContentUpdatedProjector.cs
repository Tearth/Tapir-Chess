﻿using Dapper;
using MediatR;
using Tapir.Core.Interfaces;
using Tapir.Services.News.Domain.News.Events;

namespace Tapir.Services.News.Application.Projectors
{
    public class NewsContentUpdatedProjector : INotificationHandler<NewsContentUpdatedEvent>
    {
        private readonly IDatabaseConnection _database;

        public NewsContentUpdatedProjector(IDatabaseConnection database)
        {
            _database = database;
        }

        public async Task Handle(NewsContentUpdatedEvent notification, CancellationToken cancellationToken)
        {
            using (var connection = _database.Open())
            {
                await connection.ExecuteAsync("UPDATE News SET Content = @Content, UpdatedAt = NOW() WHERE AggregateId = @AggregateId", new
                {
                    notification.Content,
                    notification.AggregateId,
                });
            }
        }
    }
}
