using Microsoft.AspNetCore.SignalR;

namespace Tapir.Services.Games.API.Controllers
{
    public class WebSocketController : Hub
    {
        public WebSocketController()
        {

        }

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }
    }
}
