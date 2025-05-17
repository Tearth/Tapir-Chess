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
            var entity = new PlayerEntity();
            var username = "Username";

            // Act
            entity.SetUsername(username);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(entity.Username, Is.EqualTo(username));
                Assert.That(entity.GetUncommittedEvents().Count, Is.EqualTo(1));
                Assert.That(entity.GetUncommittedEvents<PlayerUsernameUpdatedEvent>()[0].Username, Is.EqualTo(username));
            });
        }

        [Test]
        public void SetUsername_EmptyUsername_ThrowsDomainException()
        {
            // Arrange
            var entity = new PlayerEntity();

            // Act
            var act = () => entity.SetUsername("");

            // Assert
            Assert.That(act, Throws.TypeOf<DomainException>());
        }
    }
}