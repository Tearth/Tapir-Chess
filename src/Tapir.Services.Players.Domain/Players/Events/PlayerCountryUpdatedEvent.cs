using Tapir.Core.Domain;

namespace Tapir.Services.Players.Domain.Players.Events
{
    public class PlayerCountryUpdatedEvent : DomainEvent
    {
        public string Country { get; set; }

        public PlayerCountryUpdatedEvent(Guid aggregateId, string country) : base(aggregateId)
        {
            Country = country;
        }
    }
}
