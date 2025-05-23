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
        public int MovesNumber { get; private set; }
        public DateTime? LastMoveMadeAt { get; private set; }
        public SideToMove SideToMove { get; private set; }
        public GameResult Result { get; private set; }
        public GameStatus Status { get; private set; }
        public string Pgn { get; private set; }
        public string Fen { get; private set; }

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

            var fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
            var @event = new GameCreatedEvent(id, DateTime.UtcNow, userIdWhite, userIdBlack, usernameWhite, usernameBlack, timeControl, fen);

            ApplyEvent(@event);
            ApplyUncommittedEvent(@event);
        }

        public void MakeMove(string move, string moveShort, DateTime madeAt, string fen)
        {
            if (string.IsNullOrEmpty(move))
            {
                throw new DomainException("Move is required.", "EmptyMove");
            }

            if (string.IsNullOrEmpty(moveShort))
            {
                throw new DomainException("Move in short notation is required.", "EmptyMoveShort");
            }

            if (string.IsNullOrEmpty(fen))
            {
                throw new DomainException("FEN is required.", "EmptyFen");
            }

            var timeWhite = TimeWhite;
            var timeBlack = TimeBlack;
            var moveTime = (int)(madeAt - (LastMoveMadeAt ?? madeAt)).TotalMilliseconds;

            switch (SideToMove)
            {
                case SideToMove.White:
                {
                    timeWhite = Math.Max(0, timeWhite - moveTime);

                    if (timeWhite > 0)
                    {
                        timeWhite += TimeControl.Increment * 1000;
                    }

                    break;
                }
                case SideToMove.Black:
                {
                    timeBlack = Math.Max(0, timeBlack - moveTime);

                    if (timeBlack > 0)
                    {
                        timeBlack += TimeControl.Increment * 1000;
                    }

                    break;
                }
            }

            if (SideToMove == SideToMove.White && MovesNumber == 0)
            {
                var @event = new GameStartedEvent(Id);

                ApplyEvent(@event);
                ApplyUncommittedEvent(@event);
            }

            {
                var moveNumber = SideToMove == SideToMove.White ? MovesNumber : MovesNumber + 1;
                var @event = new MoveMadeEvent(Id, move, moveShort, madeAt, SideToMove, timeWhite, timeBlack, moveNumber, fen);

                ApplyEvent(@event);
                ApplyUncommittedEvent(@event);
            }
        }

        public override void ApplyEvent(DomainEvent @event)
        {
            switch (@event)
            {
                case GameCreatedEvent gameCreatedEvent: ExecuteEvent(gameCreatedEvent); break;
                case GameStartedEvent gameStartedEvent: ExecuteEvent(gameStartedEvent); break;
                case MoveMadeEvent moveMadeEvent: ExecuteEvent(moveMadeEvent); break;
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
            TimeWhite = @event.TimeControl.Time * 60 * 1000;
            TimeBlack = @event.TimeControl.Time * 60 * 1000;
            Pgn = "";
            Fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
        }

        private void ExecuteEvent(GameStartedEvent @event)
        {
            Status = GameStatus.InProgress;
        }

        private void ExecuteEvent(MoveMadeEvent @event)
        {
            LastMoveMadeAt = @event.MadeAt;
            TimeWhite = @event.TimeWhite;
            TimeBlack = @event.TimeBlack;
            MovesNumber = @event.MoveNumber;

            if (SideToMove == SideToMove.White)
            {
                if (!string.IsNullOrEmpty(Pgn))
                {
                    Pgn += " ";
                }

                Pgn += $"{MovesNumber + 1}.";
            }

            Pgn += $" {@event.MoveShort}";
            Fen = @event.Fen;

            switch (SideToMove)
            {
                case SideToMove.White: SideToMove = SideToMove.Black; break;
                case SideToMove.Black: SideToMove = SideToMove.White; break;
            }
        }
    }
}
