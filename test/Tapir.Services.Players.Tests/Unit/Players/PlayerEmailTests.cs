using Tapir.Core.Domain.Exceptions;
using Tapir.Services.Players.Domain.Players.Entities;
using Tapir.Services.Players.Domain.Players.Events;

namespace Tapir.Services.Players.Tests.Unit.Players
{
    public class PlayerEmailTests
    {
        [Test]
        public void SetEmail_ValidEmail_EventEmitted()
        {
            // Arrange
            var newsEntity = new PlayerEntity();
            var email = "test@test.pl";

            // Act
            newsEntity.SetEmail(email);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(newsEntity.Email, Is.EqualTo(email));
                Assert.That(newsEntity.GetUncommittedEvents().Count, Is.EqualTo(1));
                Assert.That(newsEntity.GetUncommittedEvents<PlayerEmailUpdatedEvent>()[0].Email, Is.EqualTo(email));
            });
        }

        [Test]
        public void SetEmail_EmptyEmail_ThrowsDomainException()
        {
            // Arrange
            var newsEntity = new PlayerEntity();

            // Act
            var act = () => newsEntity.SetEmail("");

            // Assert
            Assert.That(act, Throws.TypeOf<DomainException>());
        }

        [Test]
        public void SetEmail_InvalidEmail_ThrowsDomainException()
        {
            // Arrange
            var newsEntity = new PlayerEntity();

            // Act
            var act = () => newsEntity.SetEmail("invalid_email@");

            // Assert
            Assert.That(act, Throws.TypeOf<DomainException>());
        }
    }
}