using Tapir.Core.Domain;

namespace Tapir.Services.Players.Domain.Players.Events
{
    public class PlayerUsernameUpdatedEvent : DomainEvent
    {
        public string Username { get; set; }

        public PlayerUsernameUpdatedEvent(Guid aggregateId, string username) : base(aggregateId)
        {
            Username = username;
        }
    }
}
