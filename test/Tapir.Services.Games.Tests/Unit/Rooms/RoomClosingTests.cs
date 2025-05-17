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
            var entity = new Room();
            var gameId = Guid.NewGuid().ToString();

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
        public void CloseRoom_AlreadyCancelledRoom_EventEmitted()
        {
            // Arrange
            var entity = new Room();
            var gameId = Guid.NewGuid().ToString();

            entity.Cancel();

            // Act
            var act = () => entity.Close(gameId);

            // Assert
            Assert.That(act, Throws.TypeOf<DomainException>());
        }


        [Test]
        public void CloseRoom_AlreadyClosedRoom_EventEmitted()
        {
            // Arrange
            var gameId = Guid.NewGuid().ToString();
            var entity = new Room();

            entity.Close(gameId);

            // Act
            var act = () => entity.Close(gameId);

            // Assert
            Assert.That(act, Throws.TypeOf<DomainException>());
        }
    }
}