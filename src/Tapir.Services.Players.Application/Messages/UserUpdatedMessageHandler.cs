using Tapir.Core.Bus;
using Tapir.Core.Messaging.Identity;
using Tapir.Services.News.Application.News.Commands;

namespace Tapir.Services.Players.Application.Messages
{
    public class UserUpdatedMessageHandler : IEventHandler<UserUpdatedMessage>
    {
        private readonly IUpdatePlayerIdentityCommandHandler _handler;

        public UserUpdatedMessageHandler(IUpdatePlayerIdentityCommandHandler handler)
        {
            _handler = handler;
        }

        public async Task Process(UserUpdatedMessage @event)
        {
            await _handler.Process(new UpdatePlayerIdentityCommand
            {
                Id = @event.Id,
                Username = @event.Username,
                Email = @event.Email
            });
        }
    }
}
