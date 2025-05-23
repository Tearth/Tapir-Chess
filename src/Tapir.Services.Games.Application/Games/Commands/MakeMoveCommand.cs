using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Tapir.Core.Chess;
using Tapir.Core.Commands;
using Tapir.Core.Domain.Exceptions;
using Tapir.Core.Identity;
using Tapir.Core.Persistence;
using Tapir.Core.Types;
using Tapir.Services.Games.Domain.Rooms.Entities;

namespace Tapir.Services.Games.Application.Games.Commands
{
    public class MakeMoveCommand
    {
        public Guid Id { get; set; }
        public string Move { get; set; }
    }

    public interface IMakeMoveCommandHandler : ICommandHandler<MakeMoveCommand, Unit>
    {

    }

    public class MakeMoveCommandHandler : IMakeMoveCommandHandler
    {
        private readonly IAggregateRepository<GameEntity> _gameRepository;
        private readonly IBoardFactory _boardFactory;

        public MakeMoveCommandHandler(IAggregateRepository<GameEntity> gameRepository, IBoardFactory boardFactory)
        {
            _gameRepository = gameRepository;
            _boardFactory = boardFactory;
        }

        public async Task<Unit> Process(MakeMoveCommand command, ClaimsPrincipal? user)
        {
            var entity = await _gameRepository.Load(command.Id);

            if (user != null)
            {
                if (entity.SideToMove == SideToMove.White && entity.UserIdWhite != user.GetId())
                {
                    throw new UnauthorizedAccessException();
                }
                else if (entity.SideToMove == SideToMove.Black && entity.UserIdBlack != user.GetId())
                {
                    throw new UnauthorizedAccessException();
                }
            }

            var board = _boardFactory.CreateFromPgn(entity.Pgn);
            var result = board.MakeMove(command.Move);

            if (result.Valid)
            {
                entity.MakeMove(command.Move, result.MoveShort, DateTime.UtcNow, result.Fen);
            }
            else
            {
                throw new DomainException("Move is invalid for the current board state.", "InvalidMove");
            }

            await _gameRepository.Save(entity);
            return Unit.Default;
        }
    }
}
