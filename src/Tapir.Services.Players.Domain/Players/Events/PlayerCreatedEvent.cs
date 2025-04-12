using Tapir.Core.Domain;

namespace Tapir.Services.Players.Domain.Players.Events
{
    public class PlayerCreatedEvent : DomainEvent
    {
        public Guid UserId { get; set; }
        public DateTime CreatedAt { get; set; }

        public PlayerCreatedEvent(Guid aggregateId, Guid userId, DateTime createdAt) : base(aggregateId)
        {
            UserId = userId;
            CreatedAt = createdAt;
        }
    }
}
