using Tapir.Core.Domain;
using Tapir.Services.Games.Domain.Rooms.Entities;

namespace Tapir.Services.Games.Domain.Rooms.Events
{
    public class MoveMadeEvent : DomainEvent
    {
        public string Move { get; set; }
        public string MoveShort { get; set; }
        public SideToMove Side { get; set; }
        public DateTime MadeAt { get; set; }
        public int TimeWhite { get; set; }
        public int TimeBlack { get; set; }
        public int MoveNumber { get; set; }
        public string Fen { get; set; }

        public MoveMadeEvent(Guid aggregateId, string move, string moveShort, DateTime madeAt, SideToMove side, int timeWhite, int timeBlack, int moveNumber, string fen) : base(aggregateId)
        {
            Move = move;
            MoveShort = moveShort;
            MadeAt = madeAt;
            Side = side;
            TimeWhite = timeWhite;
            TimeBlack = timeBlack;
            MoveNumber = moveNumber;
            Fen = fen;
        }
    }
}
