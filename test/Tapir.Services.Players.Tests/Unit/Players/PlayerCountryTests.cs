using Tapir.Core.Domain.Exceptions;
using Tapir.Services.Players.Domain.Players.Entities;
using Tapir.Services.Players.Domain.Players.Events;

namespace Tapir.Services.Players.Tests.Unit.Players
{
    public class PlayerCountryTests
    {
        [Test]
        public void SetCountry_ValidCountry_EventEmitted()
        {
            // Arrange
            var newsEntity = new PlayerEntity();
            var country = "Country";

            // Act
            newsEntity.SetCountry(country);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(newsEntity.Country, Is.EqualTo(country));
                Assert.That(newsEntity.GetUncommittedEvents().Count, Is.EqualTo(1));
                Assert.That(newsEntity.GetUncommittedEvents<PlayerCountryUpdatedEvent>()[0].Country, Is.EqualTo(country));
            });
        }

        [Test]
        public void SetCountry_EmptyCountry_ThrowsDomainException()
        {
            // Arrange
            var newsEntity = new PlayerEntity();

            // Act
            var act = () => newsEntity.SetCountry("");

            // Assert
            Assert.That(act, Throws.TypeOf<DomainException>());
        }
    }
}