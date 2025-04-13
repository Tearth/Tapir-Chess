using Dapper;
using Tapir.Core.Bus;
using Tapir.Core.Persistence;
using Tapir.Services.Players.Domain.Players.Events;

namespace Tapir.Services.Players.Application.Players.Projectors
{
    public class PlayerCreatedProjector : IEventHandler<PlayerCreatedEvent>
    {
        private readonly IDatabaseConnection _database;

        public PlayerCreatedProjector(IDatabaseConnection database)
        {
            _database = database;
        }

        public async Task Process(PlayerCreatedEvent @event)
        {
            using (var connection = _database.Open())
            {
                await connection.ExecuteAsync("INSERT INTO Players (Id, CreatedAt, UserId) VALUES (@AggregateId, @CreatedAt, @UserId)", new
                {
                    @event.AggregateId,
                    @event.CreatedAt,
                    @event.UserId
                });
            }
        }
    }
}
