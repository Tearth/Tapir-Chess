using Tapir.Core.Domain;
using Tapir.Services.Games.Domain.Rooms.ValueObjects;

namespace Tapir.Services.Games.Domain.Rooms.Events
{
    public class RoomCreatedEvent : DomainEvent
    {
        public DateTime CreatedAt { get; set; }
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public TimeControl TimeControl { get; set; }

        public RoomCreatedEvent(Guid aggregateId, DateTime createdAt, Guid userId, string username, TimeControl timeControl) : base(aggregateId)
        {
            CreatedAt = createdAt;
            UserId = userId;
            Username = username;
            TimeControl = timeControl;
        }
    }
}
