using Tapir.Core.Domain.Exceptions;
using Tapir.Services.News.Domain.News.Entities;
using Tapir.Services.News.Domain.News.Events;

namespace Tapir.Services.News.Tests.Unit.News
{
    public class NewsAliasTests
    {
        [Test]
        public void SetAlias_ValidAlias_EventEmitted()
        {
            // Arrange
            var newsEntity = new NewsEntity();
            var alias = "correct-alias";

            // Act
            newsEntity.SetAlias(alias);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(newsEntity.Alias, Is.EqualTo(alias));
                Assert.That(newsEntity.GetUncommittedEvents().Count, Is.EqualTo(1));
                Assert.That(newsEntity.GetUncommittedEvents<NewsAliasUpdatedEvent>()[0].Alias, Is.EqualTo(alias));
            });
        }

        [Test]
        public void SetAlias_EmptyAlias_ThrowsDomainException()
        {
            // Arrange
            var newsEntity = new NewsEntity();

            // Act
            var act = () => newsEntity.SetAlias("");

            // Assert
            Assert.That(act, Throws.TypeOf<DomainException>());
        }

        [Test]
        public void SetAlias_AliasWithWhitespace_ThrowsDomainException()
        {
            // Arrange
            var newsEntity = new NewsEntity();

            // Act
            var act = () => newsEntity.SetAlias("alias with whitespaces");

            // Assert
            Assert.That(act, Throws.TypeOf<DomainException>());
        }

        [Test]
        public void SetAlias_AliasWithUppercase_ThrowsDomainException()
        {
            // Arrange
            var newsEntity = new NewsEntity(Guid.NewGuid());

            // Act
            var act = () => newsEntity.SetAlias("Alias");

            // Assert
            Assert.That(act, Throws.TypeOf<DomainException>());
        }
    }
}
