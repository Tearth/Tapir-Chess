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
            var entity = new NewsEntity();
            var title = "Title";

            // Act
            entity.SetTitle(title);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(entity.Title, Is.EqualTo(title));
                Assert.That(entity.GetUncommittedEvents().Count, Is.EqualTo(1));
                Assert.That(entity.GetUncommittedEvents<NewsTitleUpdatedEvent>()[0].Title, Is.EqualTo(title));
            });
        }

        [Test]
        public void SetTitle_EmptyTitle_ThrowsDomainException()
        {
            // Arrange
            var entity = new NewsEntity();

            // Act
            var act = () => entity.SetTitle("");

            // Assert
            Assert.That(act, Throws.TypeOf<DomainException>());
        }
    }
}
