using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Tapir.Core.Commands;
using Tapir.Core.Identity;
using Tapir.Core.Persistence;
using Tapir.Core.Types;
using Tapir.Services.Games.Domain.Rooms.Entities;
using Tapir.Services.Games.Domain.Rooms.ValueObjects;

namespace Tapir.Services.Games.Application.Games.Commands
{
    public class CreateGameCommand
    {
        public Guid UserIdWhite { get; set; }
        public Guid UserIdBlack { get; set; }
        public string UsernameWhite { get; set; }
        public string UsernameBlack { get; set; }
        public required int Time { get; set; }
        public required int Increment { get; set; }
    }

    public class CreateGameCommandResult
    {
        public Guid Id { get; set; }
    }

    public interface ICreateGameCommandHandler : ICommandHandler<CreateGameCommand, CreateGameCommandResult>
    {

    }

    public class CreateGameCommandHandler : ICreateGameCommandHandler
    {
        private readonly IAggregateRepository<GameEntity> _gameRepository;

        public CreateGameCommandHandler(IAggregateRepository<GameEntity> gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public async Task<CreateGameCommandResult> Process(CreateGameCommand command, ClaimsPrincipal? user)
        {
            var entity = new GameEntity(
                Guid.NewGuid(), 
                command.UserIdWhite,
                command.UserIdBlack, 
                command.UsernameWhite,
                command.UsernameBlack, 
                new TimeControl(command.Time, command.Increment)
            );
            
            await _gameRepository.Save(entity);

            return new CreateGameCommandResult
            {
                Id = entity.Id
            };
        }
    }
}
