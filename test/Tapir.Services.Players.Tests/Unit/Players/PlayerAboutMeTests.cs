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
            var newsEntity = new PlayerEntity();
            var aboutMe = "AboutMe";

            // Act
            newsEntity.SetAboutMe(aboutMe);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(newsEntity.AboutMe, Is.EqualTo(aboutMe));
                Assert.That(newsEntity.GetUncommittedEvents().Count, Is.EqualTo(1));
                Assert.That(newsEntity.GetUncommittedEvents<PlayerAboutMeUpdatedEvent>()[0].AboutMe, Is.EqualTo(aboutMe));
            });
        }

        [Test]
        public void SetAboutMe_EmptyAboutMe_ThrowsDomainException()
        {
            // Arrange
            var newsEntity = new PlayerEntity();

            // Act
            var act = () => newsEntity.SetAboutMe("");

            // Assert
            Assert.That(act, Throws.TypeOf<DomainException>());
        }
    }
}