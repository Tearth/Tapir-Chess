using Dapper;
using Tapir.Core.Bus;
using Tapir.Core.Persistence;
using Tapir.Services.Players.Domain.Players.Events;

namespace Tapir.Services.News.Application.News.Projectors
{
    public class PlayerAboutMeUpdatedProjector : IEventHandler<PlayerAboutMeUpdatedEvent>
    {
        private readonly IDatabaseConnection _database;

        public PlayerAboutMeUpdatedProjector(IDatabaseConnection database)
        {
            _database = database;
        }

        public async Task Process(PlayerAboutMeUpdatedEvent @event)
        {
            using (var connection = _database.Open())
            {
                await connection.ExecuteAsync("UPDATE Players SET AboutMe = @AboutMe, UpdatedAt = NOW() WHERE Id = @AggregateId", new
                {
                    @event.AboutMe,
                    @event.AggregateId,
                });
            }
        }
    }
}
