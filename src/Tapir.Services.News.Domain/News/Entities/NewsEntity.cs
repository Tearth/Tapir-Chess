using Tapir.Core.Domain;
using Tapir.Core.Exceptions;
using Tapir.Services.News.Domain.News.Events;

namespace Tapir.Services.News.Domain.News.Entities
{
    public class NewsEntity : AggregateRoot
    {
        public DateTime CreatedAt { get; private set; }
        public string Title { get; private set; }
        public string Alias { get; private set; }
        public string Content { get; private set; }

        public NewsEntity()
        {
            var @event = new NewsCreatedEvent(Id, DateTime.UtcNow);

            ApplyEvent(@event);
            ApplyUncommittedEvent(@event);
        }

        public NewsEntity(Guid id) : base(id)
        {

        }

        public void SetTitle(string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new DomainException("Title is required.");
            }

            var @event = new NewsTitleUpdatedEvent(Id, title);

            ApplyEvent(@event);
            ApplyUncommittedEvent(@event);
        }

        public void SetAlias(string alias)
        {
            if (string.IsNullOrEmpty(alias))
            {
                throw new DomainException("Alias is required.");
            }

            var @event = new NewsAliasUpdatedEvent(Id, alias);

            ApplyEvent(@event);
            ApplyUncommittedEvent(@event);
        }

        public void SetContent(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                throw new DomainException("Content is required.");
            }

            var @event = new NewsContentUpdatedEvent(Id, content);

            ApplyEvent(@event);
            ApplyUncommittedEvent(@event);
        }

        public override void ApplyEvent(DomainEvent @event)
        {
            switch (@event)
            {
                case NewsCreatedEvent newsCreatedEvent: ExecuteEvent(newsCreatedEvent); break;
                case NewsTitleUpdatedEvent titleUpdatedEvent: ExecuteEvent(titleUpdatedEvent); break;
                case NewsAliasUpdatedEvent titleAliasEvent: ExecuteEvent(titleAliasEvent); break;
                case NewsContentUpdatedEvent titleContentEvent: ExecuteEvent(titleContentEvent); break;
            }
        }

        private void ExecuteEvent(NewsCreatedEvent @event)
        {
            CreatedAt = @event.CreatedAt;
        }

        private void ExecuteEvent(NewsTitleUpdatedEvent @event)
        {
            Title = @event.Title;
        }

        private void ExecuteEvent(NewsAliasUpdatedEvent @event)
        {
            Alias = @event.Alias;
        }

        private void ExecuteEvent(NewsContentUpdatedEvent @event)
        {
            Content = @event.Content;
        }
    }
}
