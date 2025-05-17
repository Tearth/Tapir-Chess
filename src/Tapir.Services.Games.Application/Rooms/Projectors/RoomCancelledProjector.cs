using Dapper;
using Tapir.Core.Bus;
using Tapir.Core.Persistence;
using Tapir.Services.Games.Domain.Rooms.Entities;
using Tapir.Services.Games.Domain.Rooms.Events;

namespace Tapir.Services.Games.Application.Rooms.Projectors
{
    public class RoomCancelledProjector : IEventHandler<RoomCancelledEvent>
    {
        private readonly IDatabaseConnection _database;

        public RoomCancelledProjector(IDatabaseConnection database)
        {
            _database = database;
        }

        public async Task Process(RoomCancelledEvent @event)
        {
            using (var connection = _database.Open())
            {
                await connection.ExecuteAsync("UPDATE Rooms SET Status = @Status, UpdatedAt = NOW() WHERE Id = @AggregateId", new
                {
                    @event.AggregateId,
                    Status = RoomStatus.Cancelled,
                });
            }
        }
    }
}
