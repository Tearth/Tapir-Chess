using Microsoft.AspNetCore.Http;
using Tapir.Core.Commands;
using Tapir.Core.Identity;
using Tapir.Core.Persistence;
using Tapir.Core.Types;
using Tapir.Services.Players.Domain.Players.Entities;

namespace Tapir.Services.News.Application.News.Commands
{
    public class UpdatePlayerIdentityCommand
    {
        public required Guid Id { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
    }

    public interface IUpdatePlayerIdentityCommandHandler : ICommandHandler<UpdatePlayerIdentityCommand, Unit>
    {

    }

    public class UpdatePlayerIdentityCommandHandler : IUpdatePlayerIdentityCommandHandler
    {
        private readonly IAggregateRepository<PlayerEntity> _playerRepository;

        public UpdatePlayerIdentityCommandHandler(IAggregateRepository<PlayerEntity> newsRepository)
        {
            _playerRepository = newsRepository;
        }

        public async Task<Unit> Process(UpdatePlayerIdentityCommand command)
        {
            var entity = await _playerRepository.Load(command.Id);

            if (command.Username != entity.Username)
            {
                entity.SetUsername(command.Username);
            }

            if (command.Email != entity.Email)
            {
                entity.SetEmail(command.Email);
            }

            await _playerRepository.Save(entity);
            return Unit.Default;
        }
    }
}
