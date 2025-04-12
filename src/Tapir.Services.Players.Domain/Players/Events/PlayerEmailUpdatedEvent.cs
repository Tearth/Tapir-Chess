using Tapir.Core.Domain;

namespace Tapir.Services.Players.Domain.Players.Events
{
    public class PlayerEmailUpdatedEvent : DomainEvent
    {
        public string Email { get; set; }

        public PlayerEmailUpdatedEvent(Guid aggregateId, string email) : base(aggregateId)
        {
            Email = email;
        }
    }
}
