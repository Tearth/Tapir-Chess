using Dapper;
using Tapir.Core.Bus;
using Tapir.Core.Persistence;
using Tapir.Services.Players.Domain.Players.Events;

namespace Tapir.Services.News.Application.News.Projectors
{
    public class PlayerCountryUpdatedProjector : IEventHandler<PlayerCountryUpdatedEvent>
    {
        private readonly IDatabaseConnection _database;

        public PlayerCountryUpdatedProjector(IDatabaseConnection database)
        {
            _database = database;
        }

        public async Task Process(PlayerCountryUpdatedEvent @event)
        {
            using (var connection = _database.Open())
            {
                await connection.ExecuteAsync("UPDATE Players SET Country = @Country, UpdatedAt = NOW() WHERE Id = @AggregateId", new
                {
                    @event.Country,
                    @event.AggregateId,
                });
            }
        }
    }
}
