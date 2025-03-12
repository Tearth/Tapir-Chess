namespace Tapir.Core.Interfaces
{
    public interface IDomainEventSynchronizer
    {
        Task PublishUncommittedEvents(DateTime? from = null, DateTime? to = null);
    }
}
