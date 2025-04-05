namespace Tapir.Core.Events
{
    public interface IDomainEventBus
    {
        Task Send<TEvent>(TEvent @event) where TEvent : notnull;
    }
}
