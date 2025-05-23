using Tapir.Core.Domain;
using Tapir.Services.Games.Domain.Rooms.ValueObjects;

namespace Tapir.Services.Games.Domain.Rooms.Events
{
    public class GameStartedEvent : DomainEvent
    {
        public DateTime CreatedAt { get; set; }

        public GameStartedEvent(Guid aggregateId) : base(aggregateId)
        {

        }
    }
}
