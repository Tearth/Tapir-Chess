using Tapir.Core.Bus;
using Tapir.Core.Messaging.Identity;
using Tapir.Services.News.Application.News.Commands;

namespace Tapir.Services.Players.Application.Messages
{
    public class UserCreatedMessageHandler : IEventHandler<UserCreatedMessage>
    {
        private readonly ICreatePlayerCommandHandler _handler;

        public UserCreatedMessageHandler(ICreatePlayerCommandHandler handler)
        {
            _handler = handler;
        }

        public async Task Process(UserCreatedMessage @event)
        {
            await _handler.Process(new CreatePlayerCommand
            {
                UserId = @event.Id,
                Username = @event.Username,
                Email = @event.Email
            });
        }
    }
}
