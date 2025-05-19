using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Tapir.Core.Identity;
using Tapir.Services.Games.Application.Games.Queries;

namespace Tapir.Services.Games.API.Hubs
{
    public class WebSocketHub : Hub
    {
        public WebSocketHub()
        {

        }

        public override async Task OnConnectedAsync()
        {
            if (Context.User?.GetId() is Guid userId)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, userId.ToString());
            }
            
            await base.OnConnectedAsync();
        }

        public async Task GetGameInfo(Guid id, [FromServices] IGetGameLiveQueryHandler handler)
        {
            var result = await handler.Process(new GetGameLiveQuery
            {
                Id = id
            }, null);

            await Clients.Client(Context.ConnectionId).SendAsync("onGameInfo", result);
        }
    }
}
