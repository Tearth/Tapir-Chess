using Tapir.Core.Domain.Exceptions;
using Tapir.Services.Games.Domain.Rooms.Entities;
using Tapir.Services.Games.Domain.Rooms.Events;
using Tapir.Services.Games.Domain.Rooms.ValueObjects;

namespace Tapir.Services.Games.Tests.Unit.Rooms
{
    public class RoomCancellationTests
    {
        [Test]
        public void CancelRoom_ValidRoom_EventEmitted()
        {
            // Arrange
            var entity = new Room();

            // Act
            entity.Cancel();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(entity.Status, Is.EqualTo(RoomStatus.Cancelled));
                Assert.That(entity.GetUncommittedEvents().Count, Is.EqualTo(1));
                Assert.That(entity.GetUncommittedEvents<RoomCancelledEvent>().Count, Is.EqualTo(1));
            });
        }

        [Test]
        public void CancelRoom_AlreadyCancelledRoom_EventEmitted()
        {
            // Arrange
            var entity = new Room();
            entity.Cancel();

            // Act
            var act = () => entity.Cancel();

            // Assert
            Assert.That(act, Throws.TypeOf<DomainException>());
        }


        [Test]
        public void CancelRoom_AlreadyClosedRoom_EventEmitted()
        {
            // Arrange
            var entity = new Room();
            var gameId = Guid.NewGuid();

            entity.Close(gameId);

            // Act
            var act = () => entity.Cancel();

            // Assert
            Assert.That(act, Throws.TypeOf<DomainException>());
        }
    }
}