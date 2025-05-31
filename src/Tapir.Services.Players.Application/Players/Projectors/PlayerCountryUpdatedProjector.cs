using Dapper;
using Tapir.Core.Bus;
using Tapir.Core.Persistence;
using Tapir.Services.Players.Domain.Players.Events;

namespace Tapir.Services.Players.Application.Players.Projectors
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
                var table = $"Players{(@event.IsReplay() ? "_Rebuild" : "")}";

                await connection.ExecuteAsync($"UPDATE {table} SET Country = @Country, UpdatedAt = NOW() WHERE Id = @AggregateId", new
                {
                    @event.Country,
                    @event.AggregateId,
                });
            }
        }
    }
}
