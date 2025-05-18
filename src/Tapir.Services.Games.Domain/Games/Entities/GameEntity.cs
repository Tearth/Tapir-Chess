using Tapir.Core.Domain.Exceptions;
using Tapir.Core.Domain;
using Tapir.Services.Games.Domain.Rooms.ValueObjects;
using Tapir.Services.Games.Domain.Rooms.Events;

namespace Tapir.Services.Games.Domain.Rooms.Entities
{
    public enum GameStatus
    {
        Initialized,
        InProgress,
        Finished
    }

    public enum GameResult
    {
        None,
        WhiteWon,
        BlackWon,
        Draw
    }

    public enum SideToMove
    {
        White,
        Black
    }

    public class GameEntity : AggregateRoot
    {
        public DateTime CreatedAt { get; private set; }
        public Guid UserIdWhite { get; private set; }
        public Guid UserIdBlack { get; private set; }
        public string UsernameWhite { get; private set; }
        public string UsernameBlack { get; private set; }
        public TimeControl TimeControl { get; private set; }
        public int TimeWhite { get; private set; }
        public int TimeBlack { get; private set; }
        public string Pgn { get; private set; }
        public SideToMove SideToMove { get; private set; }
        public GameResult Result { get; private set; }
        public GameStatus Status { get; private set; }

        public GameEntity()
        {

        }

        public GameEntity(Guid id, Guid userIdWhite, Guid userIdBlack, string usernameWhite, string usernameBlack, TimeControl timeControl)
        {
            if (string.IsNullOrEmpty(usernameWhite))
            {
                throw new DomainException("White player's username is required.", "EmptyUsername");
            }

            if (string.IsNullOrEmpty(usernameBlack))
            {
                throw new DomainException("Black player's username is required.", "EmptyUsername");
            }

            if (timeControl.Time == 0 && timeControl.Increment == 0)
            {
                throw new DomainException("Time control can't contain only zero values.", "ZeroTimeAndIncrement");
            }

            if (timeControl.Time < 0 || timeControl.Increment < 0)
            {
                throw new DomainException("Time control can't contain negative values.", "NegativeTimeOrIncrement");
            }

            if (timeControl.Time > 60 * 60 * 24)
            {
                throw new DomainException("Time is too high.", "TimeTooHigh");
            }

            if (timeControl.Increment > 60 * 60 * 24)
            {
                throw new DomainException("Increment is too high.", "IncrementTooHigh");
            }

            var @event = new GameCreatedEvent(id, DateTime.UtcNow, userIdWhite, userIdBlack, usernameWhite, usernameBlack, timeControl);

            ApplyEvent(@event);
            ApplyUncommittedEvent(@event);
        }

        public override void ApplyEvent(DomainEvent @event)
        {
            switch (@event)
            {
                case GameCreatedEvent gameCreatedEvent: ExecuteEvent(gameCreatedEvent); break;
            }

            base.ApplyEvent(@event);
        }

        private void ExecuteEvent(GameCreatedEvent @event)
        {
            Id = @event.AggregateId;
            CreatedAt = @event.CreatedAt;
            UserIdWhite = @event.UserIdWhite;
            UserIdBlack = @event.UserIdBlack;
            UsernameWhite = @event.UsernameWhite;
            UsernameBlack = @event.UsernameBlack;
            TimeControl = @event.TimeControl;
            TimeWhite = @event.TimeControl.Time * 1000;
            TimeBlack = @event.TimeControl.Time * 1000;
        }
    }
}
