using Dapper;
using Tapir.Core.Bus;
using Tapir.Core.Persistence;
using Tapir.Services.Games.Domain.Rooms.Events;

namespace Tapir.Services.Games.Application.Rooms.Projectors
{
    public class RoomCreatedProjector : IEventHandler<RoomCreatedEvent>
    {
        private readonly IDatabaseConnection _database;

        public RoomCreatedProjector(IDatabaseConnection database)
        {
            _database = database;
        }

        public async Task Process(RoomCreatedEvent @event)
        {
            using (var connection = _database.Open())
            {
                await connection.ExecuteAsync(
                    "INSERT INTO Rooms (Id, CreatedAt, UserId, Username, Time, Increment) " +
                    "VALUES (@AggregateId, @CreatedAt, @UserId, @Username, @Time, @Increment) " +
                    "ON CONFLICT (Id) DO NOTHING", 
                new
                {
                    @event.AggregateId,
                    @event.CreatedAt,
                    @event.UserId,
                    @event.Username,
                    @event.TimeControl.Time,
                    @event.TimeControl.Increment
                });
            }
        }
    }
}
