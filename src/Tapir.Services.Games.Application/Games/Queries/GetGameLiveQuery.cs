using Dapper;
using System.Security.Claims;
using Tapir.Core.Commands;
using Tapir.Core.Persistence;
using Tapir.Services.Games.Application.Games.Queries.DTOs;
using Tapir.Services.Games.Application.Rooms.Queries;
using Tapir.Services.Games.Application.Rooms.Queries.DTOs;
using Tapir.Services.Games.Domain.Rooms.Entities;

namespace Tapir.Services.Games.Application.Games.Queries
{
    public class GetGameLiveQuery
    {
        public Guid Id { get; set; }
    }

    public interface IGetGameLiveQueryHandler : ICommandHandler<GetGameLiveQuery, GameDto?>
    {

    }

    public class GetGameLiveHandler : IGetGameLiveQueryHandler
    {
        private readonly IAggregateRepository<GameEntity> _gameRepository;

        public GetGameLiveHandler(IAggregateRepository<GameEntity> gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public async Task<GameDto?> Process(GetGameLiveQuery query, ClaimsPrincipal? user)
        {
            var entity = await _gameRepository.Load(query.Id);

            return new GameDto
            {
                CreatedAt = entity.CreatedAt,
                UserIdWhite = entity.UserIdWhite,
                UserIdBlack = entity.UserIdBlack,
                UsernameWhite = entity.UsernameWhite,
                UsernameBlack = entity.UsernameBlack,
                Time = entity.TimeControl.Time,
                Increment = entity.TimeControl.Increment,
                TimeWhite = entity.TimeWhite,
                TimeBlack = entity.TimeBlack,
                Pgn = entity.Pgn,
                SideToMove = entity.SideToMove,
                Result = entity.Result,
                Status = entity.Status
            };
        }
    }
}
