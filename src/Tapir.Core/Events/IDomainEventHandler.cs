namespace Tapir.Core.Events
{
    public interface IDomainEventHandler<TEvent>
    {
        Task Process(TEvent @event);
    }
}
