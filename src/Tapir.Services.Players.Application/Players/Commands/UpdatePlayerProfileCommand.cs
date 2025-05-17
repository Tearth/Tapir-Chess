using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Tapir.Core.Commands;
using Tapir.Core.Identity;
using Tapir.Core.Persistence;
using Tapir.Core.Types;
using Tapir.Services.Players.Domain.Players.Entities;

namespace Tapir.Services.News.Application.News.Commands
{
    public class UpdatePlayerProfileCommand
    {
        public required Guid Id { get; set; }
        public string? Country { get; set; }
        public string? AboutMe { get; set; }
    }

    public interface IUpdatePlayerProfileCommandHandler : ICommandHandler<UpdatePlayerProfileCommand, Unit>
    {

    }

    public class UpdatePlayerProfileCommandHandler : IUpdatePlayerProfileCommandHandler
    {
        private readonly IAggregateRepository<PlayerEntity> _playerRepository;

        public UpdatePlayerProfileCommandHandler(IAggregateRepository<PlayerEntity> newsRepository)
        {
            _playerRepository = newsRepository;
        }

        public async Task<Unit> Process(UpdatePlayerProfileCommand command, ClaimsPrincipal? user)
        {
            var entity = await _playerRepository.Load(command.Id);
            var userId = user.GetId();
            
            if (userId == null || entity.Id != userId)
            {
                throw new UnauthorizedAccessException();
            }

            if (command.Country != null)
            {
                entity.SetCountry(command.Country);
            }

            if (command.AboutMe != null)
            {
                entity.SetAboutMe(command.AboutMe);
            }

            await _playerRepository.Save(entity);
            return Unit.Default;
        }
    }
}
