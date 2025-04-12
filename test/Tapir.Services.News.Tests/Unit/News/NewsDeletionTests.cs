using Tapir.Core.Domain.Exceptions;
using Tapir.Services.News.Domain.News.Entities;
using Tapir.Services.News.Domain.News.Events;

namespace Tapir.Services.News.Tests.Unit.News
{
    public class NewsDeletionTests
    {
        [Test]
        public void DeleteNews_ValidNews_EventEmitted()
        {
            // Arrange
            var newsEntity = new NewsEntity();

            // Act
            newsEntity.Delete();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(newsEntity.Deleted, Is.True);
                Assert.That(newsEntity.GetUncommittedEvents().Count, Is.EqualTo(1));
                Assert.That(newsEntity.GetUncommittedEvents<NewsDeletedEvent>()[0], Is.Not.Null);
            });
        }

        [Test]
        public void DeleteNews_AlreadyDeletedNews_ThrowsDomainException()
        {
            // Arrange
            var newsEntity = new NewsEntity();
            newsEntity.Delete();

            // Act
            var act = () => newsEntity.Delete();

            // Assert
            Assert.That(act, Throws.TypeOf<DomainException>());
        }
    }
}