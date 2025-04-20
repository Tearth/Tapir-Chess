using Microsoft.AspNetCore.Http;
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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UpdatePlayerProfileCommandHandler(IAggregateRepository<PlayerEntity> newsRepository, IHttpContextAccessor httpContextAccessor)
        {
            _playerRepository = newsRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Unit> Process(UpdatePlayerProfileCommand request)
        {
            var entity = await _playerRepository.Load(request.Id);
            var userId = _httpContextAccessor.HttpContext?.User.GetId();
            
            if (userId == null || entity.Id != Guid.Parse(userId))
            {
                throw new UnauthorizedAccessException();
            }

            if (request.Country != null)
            {
                entity.SetCountry(request.Country);
            }

            if (request.AboutMe != null)
            {
                entity.SetAboutMe(request.AboutMe);
            }

            await _playerRepository.Save(entity);
            return Unit.Default;
        }
    }
}
