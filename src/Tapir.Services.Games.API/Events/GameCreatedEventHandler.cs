using Microsoft.AspNetCore.SignalR;
using Tapir.Core.Bus;
using Tapir.Services.Games.API.Hubs;
using Tapir.Services.Games.Domain.Rooms.Events;

namespace Tapir.Services.Games.API.Events
{
    public class GameCreatedEventHandler : IEventHandler<GameCreatedEvent>
    {
        private readonly IHubContext<WebSocketHub> _hub;

        public GameCreatedEventHandler(IHubContext<WebSocketHub> hub)
        {
            _hub = hub;
        }

        public async Task Process(GameCreatedEvent @event)
        {
            var whiteGroup = _hub.Clients.Group(@event.UserIdWhite.ToString());
            var blackGroup = _hub.Clients.Group(@event.UserIdBlack.ToString());
            
            var parameters = new
            {
                id = @event.AggregateId,
                userIdWhite = @event.UserIdWhite,
                userIdBlack = @event.UserIdBlack,
                usernameWhite = @event.UsernameWhite,
                usernameBlack = @event.UsernameBlack,
            };

            await Task.WhenAll(
                whiteGroup.SendAsync("onGameCreated", parameters),
                blackGroup.SendAsync("onGameCreated", parameters)
            );
        }
    }
}
