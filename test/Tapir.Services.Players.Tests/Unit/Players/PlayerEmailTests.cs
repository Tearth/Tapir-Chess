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
            var entity = new PlayerEntity();
            var email = "test@test.pl";

            // Act
            entity.SetEmail(email);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(entity.Email, Is.EqualTo(email));
                Assert.That(entity.GetUncommittedEvents().Count, Is.EqualTo(1));
                Assert.That(entity.GetUncommittedEvents<PlayerEmailUpdatedEvent>()[0].Email, Is.EqualTo(email));
            });
        }

        [Test]
        public void SetEmail_EmptyEmail_ThrowsDomainException()
        {
            // Arrange
            var entity = new PlayerEntity();

            // Act
            var act = () => entity.SetEmail("");

            // Assert
            Assert.That(act, Throws.TypeOf<DomainException>());
        }

        [Test]
        public void SetEmail_InvalidEmail_ThrowsDomainException()
        {
            // Arrange
            var entity = new PlayerEntity();

            // Act
            var act = () => entity.SetEmail("invalid_email@");

            // Assert
            Assert.That(act, Throws.TypeOf<DomainException>());
        }
    }
}