﻿using Tapir.Core.Domain;

namespace Tapir.Services.News.Domain.News.Events
{
    public class NewsTitleUpdatedEvent : DomainEvent
    {
        public string Title { get; set; }

        public NewsTitleUpdatedEvent(Guid aggregateId, string title) : base(aggregateId)
        {
            Title = title;
        }
    }
}
