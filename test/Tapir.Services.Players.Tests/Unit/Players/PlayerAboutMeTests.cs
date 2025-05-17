using Tapir.Core.Domain.Exceptions;
using Tapir.Services.Players.Domain.Players.Entities;
using Tapir.Services.Players.Domain.Players.Events;

namespace Tapir.Services.Players.Tests.Unit.Players
{
    public class PlayerAboutMeTests
    {
        [Test]
        public void SetAboutMe_ValidAboutMe_EventEmitted()
        {
            // Arrange
            var entity = new PlayerEntity();
            var aboutMe = "AboutMe";

            // Act
            entity.SetAboutMe(aboutMe);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(entity.AboutMe, Is.EqualTo(aboutMe));
                Assert.That(entity.GetUncommittedEvents().Count, Is.EqualTo(1));
                Assert.That(entity.GetUncommittedEvents<PlayerAboutMeUpdatedEvent>()[0].AboutMe, Is.EqualTo(aboutMe));
            });
        }

        [Test]
        public void SetAboutMe_EmptyAboutMe_ThrowsDomainException()
        {
            // Arrange
            var entity = new PlayerEntity();

            // Act
            var act = () => entity.SetAboutMe("");

            // Assert
            Assert.That(act, Throws.TypeOf<DomainException>());
        }
    }
}