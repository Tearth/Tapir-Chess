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
            var entity = new PlayerEntity();
            var country = "Country";

            // Act
            entity.SetCountry(country);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(entity.Country, Is.EqualTo(country));
                Assert.That(entity.GetUncommittedEvents().Count, Is.EqualTo(1));
                Assert.That(entity.GetUncommittedEvents<PlayerCountryUpdatedEvent>()[0].Country, Is.EqualTo(country));
            });
        }

        [Test]
        public void SetCountry_EmptyCountry_ThrowsDomainException()
        {
            // Arrange
            var entity = new PlayerEntity();

            // Act
            var act = () => entity.SetCountry("");

            // Assert
            Assert.That(act, Throws.TypeOf<DomainException>());
        }
    }
}