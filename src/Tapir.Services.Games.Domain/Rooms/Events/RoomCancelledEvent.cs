using Tapir.Core.Domain;

namespace Tapir.Services.Games.Domain.Rooms.Events
{
    public class RoomCancelledEvent : DomainEvent
    {
        public RoomCancelledEvent(Guid aggregateId) : base(aggregateId)
        {

        }
    }
}
