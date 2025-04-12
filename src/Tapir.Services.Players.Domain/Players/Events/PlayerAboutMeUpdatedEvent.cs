using Tapir.Core.Domain;

namespace Tapir.Services.Players.Domain.Players.Events
{
    public class PlayerAboutMeUpdatedEvent : DomainEvent
    {
        public string AboutMe { get; set; }

        public PlayerAboutMeUpdatedEvent(Guid aggregateId, string aboutMe) : base(aggregateId)
        {
            AboutMe = aboutMe;
        }
    }
}
