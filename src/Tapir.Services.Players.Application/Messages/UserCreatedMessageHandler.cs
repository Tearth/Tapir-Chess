using Tapir.Core.Bus;
using Tapir.Core.Messaging.Identity;

namespace Tapir.Services.Players.Application.Messages
{
    public class UserCreatedMessageHandler : IEventHandler<UserCreatedMessage>
    {
        public Task Process(UserCreatedMessage @event)
        {
            return Task.CompletedTask;
        }
    }
}
