using System.Security.Claims;
using Tapir.Core.Commands;
using Tapir.Core.Persistence;
using Tapir.Core.Types;
using Tapir.Services.News.Domain.News.Entities;

namespace Tapir.Services.News.Application.News.Commands
{
    public class UpdateNewsCommand
    {
        public required Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Alias { get; set; }
        public string? Content { get; set; }
    }

    public interface IUpdateNewsCommandHandler : ICommandHandler<UpdateNewsCommand, Unit>
    {

    }

    public class UpdateNewsCommandHandler : IUpdateNewsCommandHandler
    {
        private readonly IAggregateRepository<NewsEntity> _newsRepository;

        public UpdateNewsCommandHandler(IAggregateRepository<NewsEntity> newsRepository)
        {
            _newsRepository = newsRepository;
        }

        public async Task<Unit> Process(UpdateNewsCommand command, ClaimsPrincipal? user)
        {
            var entity = await _newsRepository.Load(command.Id);

            if (command.Title != null && entity.Title != command.Title)
            {
                entity.SetTitle(command.Title);
            }

            if (command.Alias != null && entity.Alias != command.Alias)
            {
                entity.SetAlias(command.Alias);
            }

            if (command.Content != null && entity.Content != command.Content)
            {
                entity.SetContent(command.Content);
            }

            await _newsRepository.Save(entity);
            return Unit.Default;
        }
    }
}
