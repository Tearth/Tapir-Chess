using Tapir.Core.Domain;

namespace Tapir.Services.Games.Domain.Rooms.Events
{
    public class RoomClosedEvent : DomainEvent
    {
        public Guid GameId { get; set; }

        public RoomClosedEvent(Guid aggregateId, Guid gameId) : base(aggregateId)
        {
            GameId = gameId;
        }
    }
}
