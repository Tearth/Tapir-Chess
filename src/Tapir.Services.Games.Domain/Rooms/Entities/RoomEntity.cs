using Tapir.Core.Domain.Exceptions;
using Tapir.Core.Domain;
using Tapir.Services.Games.Domain.Rooms.ValueObjects;
using Tapir.Services.Games.Domain.Rooms.Events;

namespace Tapir.Services.Games.Domain.Rooms.Entities
{
    public enum RoomStatus
    {
        Open,
        Closed,
        Cancelled
    }

    public class RoomEntity : AggregateRoot
    {
        public DateTime CreatedAt { get; private set; }
        public Guid UserId { get; private set; }
        public string Username { get; private set; }
        public Guid? GameId { get; private set; }
        public TimeControl TimeControl { get; private set; }
        public RoomStatus Status { get; private set; }

        public RoomEntity()
        {

        }

        public RoomEntity(Guid id, Guid userId, string username, TimeControl timeControl)
        {
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

            var @event = new RoomCreatedEvent(id, DateTime.UtcNow, userId, username, timeControl);

            ApplyEvent(@event);
            ApplyUncommittedEvent(@event);
        }

        public void Close(Guid gameId)
        {
            if (Status == RoomStatus.Closed)
            {
                throw new DomainException("Room is already closed.", "RoomAlreadyClosed");
            }

            if (Status == RoomStatus.Cancelled)
            {
                throw new DomainException("Room is already cancelled.", "RoomAlreadyCancelled");
            }

            var @event = new RoomClosedEvent(Id, gameId);

            ApplyEvent(@event);
            ApplyUncommittedEvent(@event);
        }


        public void Cancel()
        {
            if (Status == RoomStatus.Closed)
            {
                throw new DomainException("Room is already closed.", "RoomAlreadyClosed");
            }

            if (Status == RoomStatus.Cancelled)
            {
                throw new DomainException("Room is already cancelled.", "RoomAlreadyCancelled");
            }

            var @event = new RoomCancelledEvent(Id);

            ApplyEvent(@event);
            ApplyUncommittedEvent(@event);
        }

        public override void ApplyEvent(DomainEvent @event)
        {
            switch (@event)
            {
                case RoomCreatedEvent roomCreatedEvent: ExecuteEvent(roomCreatedEvent); break;
                case RoomClosedEvent roomClosedEvent: ExecuteEvent(roomClosedEvent); break;
                case RoomCancelledEvent roomCancelledEvent: ExecuteEvent(roomCancelledEvent); break;
            }

            base.ApplyEvent(@event);
        }

        private void ExecuteEvent(RoomCreatedEvent @event)
        {
            Id = @event.AggregateId;
            CreatedAt = @event.CreatedAt;
            UserId = @event.UserId;
            Username = @event.Username;
            TimeControl = @event.TimeControl;
        }

        private void ExecuteEvent(RoomClosedEvent @event)
        {
            GameId = @event.GameId;
            Status = RoomStatus.Closed;
        }


        private void ExecuteEvent(RoomCancelledEvent @event)
        {
            Status = RoomStatus.Cancelled;
        }
    }
}
