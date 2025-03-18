using Tapir.Core.Domain.Exceptions;
using Tapir.Services.News.Domain.News.Entities;
using Tapir.Services.News.Domain.News.Events;

namespace Tapir.Services.News.Tests.Unit.News
{
    public class NewsCreationTests
    {
        [Test]
        public void Create_ValidNews_EventEmitted()
        {
            // Arrange & Act
            var newsEntity = new NewsEntity();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(newsEntity.GetUncommittedEvents().Count, Is.EqualTo(1));
                Assert.That(newsEntity.GetUncommittedEvents<NewsCreatedEvent>().Count, Is.EqualTo(1));
            });
        }
    }
}