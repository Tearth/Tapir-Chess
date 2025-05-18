using Microsoft.AspNetCore.SignalR;
using Tapir.Core.Identity;

namespace Tapir.Services.Games.API.Hubs
{
    public class WebSocketHub : Hub
    {
        public WebSocketHub()
        {

        }

        public override async Task OnConnectedAsync()
        {
            if (Context.User?.GetId() is not Guid userId)
            {
                await base.OnConnectedAsync();
                return;
            }
            
            await Groups.AddToGroupAsync(Context.ConnectionId, userId.ToString());
            await base.OnConnectedAsync();
        }
    }
}
