using Tapir.Services.Games.Domain.Rooms.Entities;
using Tapir.Services.Games.Domain.Rooms.Events;
using Tapir.Services.Games.Domain.Rooms.ValueObjects;

namespace Tapir.Services.Games.Tests.Unit.Rooms
{
    public class RoomCreationTests
    {
        [Test]
        public void CreateRoom_ValidRoom_EventEmitted()
        {
            // Arrange
            var id = Guid.NewGuid();
            var userId = Guid.NewGuid().ToString();
            var timeControl = new TimeControl(10, 5);

            // Act
            var newsEntity = new Room(id, userId, timeControl);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(newsEntity.Id, Is.EqualTo(id));
                Assert.That(newsEntity.UserId, Is.EqualTo(userId));
                Assert.That(newsEntity.TimeControl, Is.EqualTo(timeControl));
                Assert.That(newsEntity.GetUncommittedEvents().Count, Is.EqualTo(1));
                Assert.That(newsEntity.GetUncommittedEvents<RoomCreatedEvent>().Count, Is.EqualTo(1));
            });
        }
    }
}