using Tapir.Core.Domain.Exceptions;
using Tapir.Services.News.Domain.News.Entities;
using Tapir.Services.News.Domain.News.Events;

namespace Tapir.Services.News.Tests.Unit.News
{
    public class NewsContentTests
    {
        [Test]
        public void SetContent_ValidContent_EventEmitted()
        {
            // Arrange
            var newsEntity = new NewsEntity(Guid.NewGuid());
            var content = "Content";

            // Act
            newsEntity.SetContent(content);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(newsEntity.Content, Is.EqualTo(content));
                Assert.That(newsEntity.GetUncommittedEvents().Count, Is.EqualTo(1));
                Assert.That(newsEntity.GetUncommittedEvents<NewsContentUpdatedEvent>()[0].Content, Is.EqualTo(content));
            });
        }

        [Test]
        public void SetContent_EmptyContent_ThrowsDomainException()
        {
            // Arrange
            var newsEntity = new NewsEntity(Guid.NewGuid());

            // Act
            var act = () => newsEntity.SetContent("");

            // Assert
            Assert.That(act, Throws.TypeOf<DomainException>());
        }
    }
}