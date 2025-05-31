using Dapper;
using Tapir.Core.Bus;
using Tapir.Core.Persistence;
using Tapir.Services.Games.Domain.Rooms.Entities;
using Tapir.Services.Games.Domain.Rooms.Events;

namespace Tapir.Services.Games.Application.Games.Projectors
{
    public class GameStartedProjector : IEventHandler<GameStartedEvent>
    {
        private readonly IDatabaseConnection _database;

        public GameStartedProjector(IDatabaseConnection database)
        {
            _database = database;
        }

        public async Task Process(GameStartedEvent @event)
        {
            using (var connection = _database.Open())
            {
                var table = $"Games{(@event.IsReplay() ? "_Rebuild" : "")}";

                await connection.ExecuteAsync($"UPDATE {table} SET Status = @Status, UpdatedAt = NOW() WHERE Id = @AggregateId", new
                {
                    Status = GameStatus.InProgress,
                    @event.AggregateId,
                });
            }
        }
    }
}
