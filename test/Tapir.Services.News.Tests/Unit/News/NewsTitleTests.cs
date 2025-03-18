using Tapir.Core.Domain.Exceptions;
using Tapir.Services.News.Domain.News.Entities;
using Tapir.Services.News.Domain.News.Events;

namespace Tapir.Services.News.Tests.Unit.News
{
    public class NewsTitleTests
    {
        [Test]
        public void SetTitle_ValidTitle_EventEmitted()
        {
            // Arrange
            var newsEntity = new NewsEntity(Guid.NewGuid());
            var title = "Title";

            // Act
            newsEntity.SetTitle(title);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(newsEntity.Title, Is.EqualTo(title));
                Assert.That(newsEntity.GetUncommittedEvents().Count, Is.EqualTo(1));
                Assert.That(newsEntity.GetUncommittedEvents<NewsTitleUpdatedEvent>()[0].Title, Is.EqualTo(title));
            });
        }

        [Test]
        public void SetTitle_EmptyTitle_ThrowsDomainException()
        {
            // Arrange
            var newsEntity = new NewsEntity(Guid.NewGuid());

            // Act
            var act = () => newsEntity.SetTitle("");

            // Assert
            Assert.That(act, Throws.TypeOf<DomainException>());
        }
    }
}
