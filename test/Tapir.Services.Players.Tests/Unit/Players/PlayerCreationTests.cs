using Tapir.Services.Players.Domain.Players.Entities;
using Tapir.Services.Players.Domain.Players.Events;

namespace Tapir.Services.Players.Tests.Unit.Players
{
    public class PlayerCreationTests
    {
        [Test]
        public void CreatePlayer_ValidPlayer_EventEmitted()
        {
            // Arrange & Act
            var newsEntity = new PlayerEntity(Guid.NewGuid(), Guid.NewGuid());

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(newsEntity.GetUncommittedEvents().Count, Is.EqualTo(1));
                Assert.That(newsEntity.GetUncommittedEvents<PlayerCreatedEvent>().Count, Is.EqualTo(1));
            });
        }
    }
}