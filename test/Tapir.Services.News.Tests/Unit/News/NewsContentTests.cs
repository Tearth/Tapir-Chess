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
            var entity = new NewsEntity();
            var content = "Content";

            // Act
            entity.SetContent(content);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(entity.Content, Is.EqualTo(content));
                Assert.That(entity.GetUncommittedEvents().Count, Is.EqualTo(1));
                Assert.That(entity.GetUncommittedEvents<NewsContentUpdatedEvent>()[0].Content, Is.EqualTo(content));
            });
        }

        [Test]
        public void SetContent_EmptyContent_ThrowsDomainException()
        {
            // Arrange
            var entity = new NewsEntity();

            // Act
            var act = () => entity.SetContent("");

            // Assert
            Assert.That(act, Throws.TypeOf<DomainException>());
        }
    }
}