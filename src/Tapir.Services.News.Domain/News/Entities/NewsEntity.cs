using System.Text.RegularExpressions;
using Tapir.Core.Domain;
using Tapir.Core.Domain.Exceptions;
using Tapir.Services.News.Domain.News.Events;

namespace Tapir.Services.News.Domain.News.Entities
{
    public class NewsEntity : AggregateRoot
    {
        public DateTime CreatedAt { get; private set; }
        public string? Title { get; private set; }
        public string? Alias { get; private set; }
        public string? Content { get; private set; }
        public bool Deleted { get; private set; }

        public NewsEntity()
        {
        }

        public NewsEntity(Guid id)
        {
            var @event = new NewsCreatedEvent(id, DateTime.UtcNow);

            ApplyEvent(@event);
            ApplyUncommittedEvent(@event);
        }

        public void SetTitle(string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new DomainException("Title is required.", "EmptyTitle");
            }

            var @event = new NewsTitleUpdatedEvent(Id, title);

            ApplyEvent(@event);
            ApplyUncommittedEvent(@event);
        }

        public void SetAlias(string alias)
        {
            if (string.IsNullOrEmpty(alias))
            {
                throw new DomainException("Alias is required.", "EmptyAlias");
            }

            if (!Regex.IsMatch(alias, "^[a-z0-9-]+$"))
            {
                throw new DomainException("Alias can contain only lowercase letters, numbers and hyphens.", "InvalidAlias");
            }

            var @event = new NewsAliasUpdatedEvent(Id, alias);

            ApplyEvent(@event);
            ApplyUncommittedEvent(@event);
        }

        public void SetContent(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                throw new DomainException("Content is required.", "EmptyContent");
            }

            var @event = new NewsContentUpdatedEvent(Id, content);

            ApplyEvent(@event);
            ApplyUncommittedEvent(@event);
        }

        public void Delete()
        {
            if (Deleted)
            {
                throw new DomainException("News is already deleted.", "ResourceAlreadyDeleted");
            }

            var @event = new NewsDeletedEvent(Id, DateTime.UtcNow);

            ApplyEvent(@event);
            ApplyUncommittedEvent(@event);
        }

        public override void ApplyEvent(DomainEvent @event)
        {
            switch (@event)
            {
                case NewsCreatedEvent newsCreatedEvent: ExecuteEvent(newsCreatedEvent); break;
                case NewsDeletedEvent newsDeletedEvent: ExecuteEvent(newsDeletedEvent); break;
                case NewsTitleUpdatedEvent titleUpdatedEvent: ExecuteEvent(titleUpdatedEvent); break;
                case NewsAliasUpdatedEvent aliasUpdatedEvent: ExecuteEvent(aliasUpdatedEvent); break;
                case NewsContentUpdatedEvent contentUpdatedEvent: ExecuteEvent(contentUpdatedEvent); break;
            }

            base.ApplyEvent(@event);
        }

        private void ExecuteEvent(NewsCreatedEvent @event)
        {
            Id = @event.AggregateId;
            CreatedAt = @event.CreatedAt;
        }

        private void ExecuteEvent(NewsDeletedEvent @event)
        {
            Deleted = true;
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
