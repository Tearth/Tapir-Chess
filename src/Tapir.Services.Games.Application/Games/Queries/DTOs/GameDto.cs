using Tapir.Services.Games.Domain.Rooms.Entities;

namespace Tapir.Services.Games.Application.Games.Queries.DTOs
{
    public class GameDto
    {
        public DateTime CreatedAt { get; set; }
        public Guid UserIdWhite { get; set; }
        public Guid UserIdBlack { get; set; }
        public string UsernameWhite { get; set; }
        public string UsernameBlack { get; set; }
        public required int Time { get; set; }
        public required int Increment { get; set; }
        public required int TimeWhite { get; set; }
        public required int TimeBlack { get; set; }
        public string Pgn { get; set; }
        public SideToMove SideToMove { get; set; }
        public GameResult Result { get; set; }
        public GameStatus Status { get; set; }
    }
}
