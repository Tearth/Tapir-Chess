using Microsoft.AspNetCore.SignalR;
using Tapir.Core.Bus;
using Tapir.Services.Games.API.Hubs;
using Tapir.Services.Games.Application.Games.Queries;
using Tapir.Services.Games.Application.Rooms.Queries;
using Tapir.Services.Games.Domain.Rooms.Entities;
using Tapir.Services.Games.Domain.Rooms.Events;

namespace Tapir.Services.Games.API.Events
{
    public class MoveMadeEventHandler : IEventHandler<MoveMadeEvent>
    {
        private readonly IHubContext<WebSocketHub> _hub;
        private readonly IGetGameLiveQueryHandler _getGameLiveQuery;

        public MoveMadeEventHandler(IHubContext<WebSocketHub> hub, IGetGameLiveQueryHandler getGameLiveQuery)
        {
            _hub = hub;
            _getGameLiveQuery = getGameLiveQuery;
        }

        public async Task Process(MoveMadeEvent @event)
        {
            var game = await _getGameLiveQuery.Process(new GetGameLiveQuery
            {
                Id = @event.AggregateId
            }, null);

            var whiteGroup = _hub.Clients.Group(game.UserIdWhite.ToString());
            var blackGroup = _hub.Clients.Group(game.UserIdBlack.ToString());

            await whiteGroup.SendAsync("onMoveMade", @event);
            await blackGroup.SendAsync("onMoveMade", @event);
        }
    }
}
