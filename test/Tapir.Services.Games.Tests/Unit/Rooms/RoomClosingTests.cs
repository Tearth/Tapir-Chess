using Tapir.Core.Domain.Exceptions;
using Tapir.Services.Games.Domain.Rooms.Entities;
using Tapir.Services.Games.Domain.Rooms.Events;
using Tapir.Services.Games.Domain.Rooms.ValueObjects;

namespace Tapir.Services.Games.Tests.Unit.Rooms
{
    public class RoomClosingTests
    {
        [Test]
        public void CloseRoom_ValidRoom_EventEmitted()
        {
            // Arrange
            var entity = new RoomEntity();
            var gameId = Guid.NewGuid();

            // Act
            entity.Close(gameId);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(entity.GameId, Is.EqualTo(gameId));
                Assert.That(entity.Status, Is.EqualTo(RoomStatus.Closed));
                Assert.That(entity.GetUncommittedEvents().Count, Is.EqualTo(1));
                Assert.That(entity.GetUncommittedEvents<RoomClosedEvent>().Count, Is.EqualTo(1));
            });
        }

        [Test]
        public void CloseRoom_AlreadyCancelledRoom_ThrowsDomainException()
        {
            // Arrange
            var entity = new RoomEntity();
            var gameId = Guid.NewGuid();

            entity.Cancel();

            // Act
            var act = () => entity.Close(gameId);

            // Assert
            Assert.That(act, Throws.TypeOf<DomainException>());
        }


        [Test]
        public void CloseRoom_AlreadyClosedRoom_ThrowsDomainException()
        {
            // Arrange
            var gameId = Guid.NewGuid();
            var entity = new RoomEntity();

            entity.Close(gameId);

            // Act
            var act = () => entity.Close(gameId);

            // Assert
            Assert.That(act, Throws.TypeOf<DomainException>());
        }
    }
}