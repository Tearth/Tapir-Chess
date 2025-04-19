using Tapir.Core.Commands;
using Tapir.Core.Persistence;
using Tapir.Core.Types;
using Tapir.Services.Players.Domain.Players.Entities;

namespace Tapir.Services.Players.Application.Players.Commands
{
    public class CreatePlayerCommand
    {
        public required Guid UserId{ get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
    }

    public interface ICreatePlayerCommandHandler : ICommandHandler<CreatePlayerCommand, Unit>
    {

    }

    public class CreatePlayerCommandHandler : ICreatePlayerCommandHandler
    {
        private readonly IAggregateRepository<PlayerEntity> _playerRepository;

        public CreatePlayerCommandHandler(IAggregateRepository<PlayerEntity> playersRepository)
        {
            _playerRepository = playersRepository;
        }

        public async Task<Unit> Process(CreatePlayerCommand command)
        {
            var entity = new PlayerEntity(command.UserId);

            entity.SetUsername(command.Username);
            entity.SetEmail(command.Email);

            await _playerRepository.Save(entity);
            return Unit.Default;
        }
    }
}
