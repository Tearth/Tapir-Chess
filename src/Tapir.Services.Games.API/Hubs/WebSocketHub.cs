using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Tapir.Core.Identity;
using Tapir.Services.Games.Application.Games.Commands;
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
            }, Context.User);

            var parameters = new
            {
                id = id,
                createdAt = result.CreatedAt,
                userIdWhite = result.UserIdWhite,
                userIdBlack = result.UserIdBlack,
                usernameWhite = result.UsernameWhite,
                usernameBlack = result.UsernameBlack,
                time = result.Time,
                increment = result.Increment,
                timeWhite = result.TimeWhite,
                timeBlack = result.TimeBlack,
                pgn = result.Pgn,
                sideToMove = result.SideToMove,
                result = result.Result,
                status = result.Status
            };

            await Clients.Client(Context.ConnectionId).SendAsync("onGameInfo", parameters);
        }

        public async Task MakeMove(Guid id, string move, [FromServices] IMakeMoveCommandHandler handler)
        {
            var result = await handler.Process(new MakeMoveCommand
            {
                Id = id,
                Move = move
            }, Context.User);

            var whiteGroup = Clients.Group(result.UserIdWhite.ToString());
            var blackGroup = Clients.Group(result.UserIdBlack.ToString());

            var parameters = new
            {
                id = id,
                move = result.Move,
                moveShort = result.MoveShort,
                side = result.Color,
                timeWhite = result.TimeWhite,
                timeBlack = result.TimeBlack
            };

            await Task.WhenAll(
                whiteGroup.SendAsync("onMoveMade", parameters),
                blackGroup.SendAsync("onMoveMade", parameters)
            );
        }
    }
}
