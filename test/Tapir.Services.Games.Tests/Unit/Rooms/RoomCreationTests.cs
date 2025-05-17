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
            var userId = Guid.NewGuid();
            var username = "test";
            var timeControl = new TimeControl(10, 5);

            // Act
            var entity = new RoomEntity(id, userId, username, timeControl);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(entity.Id, Is.EqualTo(id));
                Assert.That(entity.UserId, Is.EqualTo(userId));
                Assert.That(entity.TimeControl, Is.EqualTo(timeControl));
                Assert.That(entity.GetUncommittedEvents().Count, Is.EqualTo(1));
                Assert.That(entity.GetUncommittedEvents<RoomCreatedEvent>().Count, Is.EqualTo(1));
            });
        }
    }
}