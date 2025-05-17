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
            var entity = new NewsEntity();

            // Act
            entity.Delete();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(entity.Deleted, Is.True);
                Assert.That(entity.GetUncommittedEvents().Count, Is.EqualTo(1));
                Assert.That(entity.GetUncommittedEvents<NewsDeletedEvent>()[0], Is.Not.Null);
            });
        }

        [Test]
        public void DeleteNews_AlreadyDeletedNews_ThrowsDomainException()
        {
            // Arrange
            var entity = new NewsEntity();
            entity.Delete();

            // Act
            var act = () => entity.Delete();

            // Assert
            Assert.That(act, Throws.TypeOf<DomainException>());
        }
    }
}