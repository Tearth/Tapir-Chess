namespace Tapir.Core.Messaging
{
    public interface IMessageBus
    {
        Task Send<T>(T message);
        Task Listen();
    }
}
