using Tapir.Core.Domain.Exceptions;
using Tapir.Services.Players.Domain.Players.Entities;
using Tapir.Services.Players.Domain.Players.Events;

namespace Tapir.Services.Players.Tests.Unit.Players
{
    public class PlayerUsernameTests
    {
        [Test]
        public void SetUsername_ValidUsername_EventEmitted()
        {
            // Arrange
            var newsEntity = new PlayerEntity();
            var username = "Username";

            // Act
            newsEntity.SetUsername(username);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(newsEntity.Username, Is.EqualTo(username));
                Assert.That(newsEntity.GetUncommittedEvents().Count, Is.EqualTo(1));
                Assert.That(newsEntity.GetUncommittedEvents<PlayerUsernameUpdatedEvent>()[0].Username, Is.EqualTo(username));
            });
        }

        [Test]
        public void SetUsername_EmptyUsername_ThrowsDomainException()
        {
            // Arrange
            var newsEntity = new PlayerEntity();

            // Act
            var act = () => newsEntity.SetUsername("");

            // Assert
            Assert.That(act, Throws.TypeOf<DomainException>());
        }
    }
}