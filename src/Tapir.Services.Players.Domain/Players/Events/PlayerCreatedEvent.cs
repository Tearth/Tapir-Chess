using Tapir.Core.Domain;

namespace Tapir.Services.Players.Domain.Players.Events
{
    public class PlayerCreatedEvent : DomainEvent
    {
        public DateTime CreatedAt { get; set; }

        public PlayerCreatedEvent(Guid aggregateId, DateTime createdAt) : base(aggregateId)
        {
            CreatedAt = createdAt;
        }
    }
}
