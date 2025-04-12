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
            var userId = Guid.NewGuid();

            // Act
            var newsEntity = new PlayerEntity(id, userId);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(newsEntity.Id, Is.EqualTo(id));
                Assert.That(newsEntity.UserId, Is.EqualTo(userId));
                Assert.That(newsEntity.GetUncommittedEvents().Count, Is.EqualTo(1));
                Assert.That(newsEntity.GetUncommittedEvents<PlayerCreatedEvent>().Count, Is.EqualTo(1));
            });
        }
    }
}