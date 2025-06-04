using Dapper;
using Tapir.Core.Bus;
using Tapir.Core.Persistence;
using Tapir.Services.Games.Domain.Rooms.Entities;
using Tapir.Services.Games.Domain.Rooms.Events;

namespace Tapir.Services.Games.Application.Rooms.Projectors
{
    public class RoomClosedProjector : IEventHandler<RoomClosedEvent>
    {
        private readonly IDatabaseConnection _database;

        public RoomClosedProjector(IDatabaseConnection database)
        {
            _database = database;
        }

        public async Task Process(RoomClosedEvent @event)
        {
            using (var connection = await _database.Open())
            {
                var table = $"Rooms{(@event.IsReplay() ? "_Rebuild" : "")}";

                await connection.ExecuteAsync($"UPDATE {table} SET GameId = @GameId, Status = @Status, UpdatedAt = NOW() WHERE Id = @AggregateId", new
                {
                    @event.GameId,
                    @event.AggregateId,
                    Status = RoomStatus.Closed,
                });
            }
        }
    }
}
