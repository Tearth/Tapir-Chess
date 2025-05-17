using System.Diagnostics.Metrics;
using Tapir.Services.Players.Domain.Players.Entities;
using Tapir.Services.Players.Domain.Players.Events;

namespace Tapir.Services.Players.Tests.Unit.Players
{
    public class PlayerCreationTests
    {
        [Test]
        public void CreatePlayer_ValidPlayer_EventEmitted()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Act
            var entity = new PlayerEntity(id);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(entity.Id, Is.EqualTo(id));
                Assert.That(entity.GetUncommittedEvents().Count, Is.EqualTo(1));
                Assert.That(entity.GetUncommittedEvents<PlayerCreatedEvent>().Count, Is.EqualTo(1));
            });
        }
    }
}