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

    public class MakeMoveCommandResult
    {
        public Guid Id { get; set; }
        public string Move { get; set; }
        public string MoveShort { get; set; }
        public Guid UserIdWhite { get; set; }
        public Guid UserIdBlack { get; set; }
        public int TimeWhite { get; set; }
        public int TimeBlack { get; set; }
        public SideToMove Color { get; set; }
    }

    public interface IMakeMoveCommandHandler : ICommandHandler<MakeMoveCommand, MakeMoveCommandResult>
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

        public async Task<MakeMoveCommandResult> Process(MakeMoveCommand command, ClaimsPrincipal? user)
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
            var sideToMove = entity.SideToMove;
            
            if (result.Valid)
            {
                entity.MakeMove(command.Move, result.MoveShort, DateTime.UtcNow, result.Fen);
            }
            else
            {
                throw new DomainException("Move is invalid for the current board state.", "InvalidMove");
            }

            await _gameRepository.Save(entity);

            return new MakeMoveCommandResult
            {
                Id = command.Id,
                Move = command.Move,
                MoveShort = result.MoveShort,
                UserIdWhite = entity.UserIdWhite,
                UserIdBlack = entity.UserIdBlack,
                TimeWhite = entity.TimeWhite,
                TimeBlack = entity.TimeBlack,
                Color = sideToMove
            };
        }
    }
}
