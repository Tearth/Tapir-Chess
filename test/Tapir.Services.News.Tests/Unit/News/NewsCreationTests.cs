using Tapir.Services.News.Domain.News.Entities;
using Tapir.Services.News.Domain.News.Events;

namespace Tapir.Services.News.Tests.Unit.News
{
    public class NewsCreationTests
    {
        [Test]
        public void CreateNews_ValidNews_EventEmitted()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Act
            var entity = new NewsEntity(id);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(entity.Id, Is.EqualTo(id));
                Assert.That(entity.GetUncommittedEvents().Count, Is.EqualTo(1));
                Assert.That(entity.GetUncommittedEvents<NewsCreatedEvent>().Count, Is.EqualTo(1));
            });
        }
    }
}