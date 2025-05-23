using Tapir.Core.Domain;
using Tapir.Services.Games.Domain.Rooms.ValueObjects;

namespace Tapir.Services.Games.Domain.Rooms.Events
{
    public class GameCreatedEvent : DomainEvent
    {
        public DateTime CreatedAt { get; set; }
        public Guid UserIdWhite { get; set; }
        public Guid UserIdBlack { get; set; }
        public string UsernameWhite { get; set; }
        public string UsernameBlack { get; set; }
        public TimeControl TimeControl { get; set; }
        public string Fen { get; set; }

        public GameCreatedEvent(Guid aggregateId, DateTime createdAt, Guid userIdWhite, Guid userIdBlack, string usernameWhite, string usernameBlack, TimeControl timeControl, string fen) : base(aggregateId)
        {
            CreatedAt = createdAt;
            UserIdWhite = userIdWhite;
            UserIdBlack = userIdBlack;
            UsernameWhite = usernameWhite;
            UsernameBlack = usernameBlack;
            TimeControl = timeControl;
            Fen = fen;
        }
    }
}
