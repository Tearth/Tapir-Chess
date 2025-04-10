namespace Tapir.Core.Messaging
{
    public interface IMessageBus
    {
        Task Send<T>(T message) where T: notnull;
        Task Listen();
    }
}
