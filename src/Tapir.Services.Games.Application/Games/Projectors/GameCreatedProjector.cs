using Dapper;
using Tapir.Core.Bus;
using Tapir.Core.Persistence;
using Tapir.Services.Games.Domain.Rooms.Events;

namespace Tapir.Services.Games.Application.Games.Projectors
{
    public class GameCreatedProjector : IEventHandler<GameCreatedEvent>
    {
        private readonly IDatabaseConnection _database;

        public GameCreatedProjector(IDatabaseConnection database)
        {
            _database = database;
        }

        public async Task Process(GameCreatedEvent @event)
        {
            using (var connection = _database.Open())
            {
                await connection.ExecuteAsync(
                    "INSERT INTO Games (Id, CreatedAt, UserIdWhite, UserIdBlack, UsernameWhite, UsernameBlack, Time, Increment, TimeWhite, TimeBlack, Fen) " +
                    "VALUES (@AggregateId, @CreatedAt, @UserIdWhite, @UserIdBlack, @UsernameWhite, @UsernameBlack, @Time, @Increment, @TimeWhite, @TimeBlack, @Fen) " +
                    "ON CONFLICT (Id) DO NOTHING", 
                new
                {
                    @event.AggregateId,
                    @event.CreatedAt,
                    @event.UserIdWhite,
                    @event.UserIdBlack,
                    @event.UsernameWhite,
                    @event.UsernameBlack,
                    @event.TimeControl.Time,
                    @event.TimeControl.Increment,
                    TimeWhite = @event.TimeControl.Time * 60 * 1000,
                    TimeBlack = @event.TimeControl.Time * 60 * 1000,
                    @event.Fen
                });
            }
        }
    }
}
