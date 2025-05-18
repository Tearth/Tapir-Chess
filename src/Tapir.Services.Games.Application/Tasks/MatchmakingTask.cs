using Tapir.Core.Bus;
using Tapir.Core.Scheduler;
using Tapir.Services.Games.Application.Games.Commands;
using Tapir.Services.Games.Application.Rooms.Commands;
using Tapir.Services.Games.Application.Rooms.Queries;

namespace Tapir.Services.Games.Application.Tasks
{
    public class MatchmakingTask : ITask
    {
        private readonly IGetRoomListQueryHandler _roomListQueryHandler;
        private readonly ICloseRoomCommandHandler _closeRoomCommandHandler;
        private readonly ICreateGameCommandHandler _createGameCommandHandler;

        public MatchmakingTask(
            IEventBus? eventBus = null,
            IGetRoomListQueryHandler? roomListQueryHandler = null,
            ICloseRoomCommandHandler? closeRoomCommandHandler = null,
            ICreateGameCommandHandler? createGameCommandHandler = null)
        {
            _roomListQueryHandler = roomListQueryHandler!;
            _closeRoomCommandHandler = closeRoomCommandHandler!;
            _createGameCommandHandler = createGameCommandHandler!;
        }

        public async Task Run()
        {
            var result = await _roomListQueryHandler.Process(new GetRoomListQuery
            {
                PageNumber = 1,
                PageSize = 1000
            }, null);

            var rooms = result.Items;
            var closed = new bool[result.Items.Count];

            for (var x = 0; x < rooms.Count; x++)
            {
                for (var y = x + 1; y < rooms.Count; y++)
                {
                    var roomA = rooms[x];
                    var roomB = rooms[y];

                    if (closed[x] || closed[y])
                    {
                        continue;
                    }

                    if (roomA.Time == roomB.Time && roomA.Increment == roomB.Increment && roomA.UserId != roomB.UserId)
                    {
                        var createGameResult = await _createGameCommandHandler.Process(new CreateGameCommand
                        {
                            UserIdWhite = roomA.UserId,
                            UserIdBlack = roomB.UserId,
                            UsernameWhite = roomA.Username,
                            UsernameBlack = roomB.Username,
                            Time = roomA.Time,
                            Increment = roomA.Increment,
                        }, null);

                        await _closeRoomCommandHandler.Process(new CloseRoomCommand
                        {
                            Id = roomA.Id,
                            GameId = createGameResult.Id
                        }, null);

                        await _closeRoomCommandHandler.Process(new CloseRoomCommand
                        {
                            Id = roomB.Id,
                            GameId = createGameResult.Id
                        }, null);

                        closed[x] = true;
                        closed[y] = true;
                    }
                }
            }
        }
    }
}
