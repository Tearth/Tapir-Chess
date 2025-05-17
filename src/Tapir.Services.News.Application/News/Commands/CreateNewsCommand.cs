using System.Security.Claims;
using Tapir.Core.Commands;
using Tapir.Core.Persistence;
using Tapir.Core.Types;
using Tapir.Services.News.Domain.News.Entities;

namespace Tapir.Services.News.Application.News.Commands
{
    public class CreateNewsCommand
    {
        public required string Title { get; set; }
        public required string Alias { get; set; }
        public required string Content { get; set; }
    }

    public class CreateNewsCommandResult
    {
        public Guid Id { get; set; }
    }

    public interface ICreateNewsCommandHandler : ICommandHandler<CreateNewsCommand, CreateNewsCommandResult>
    {

    }

    public class CreateNewsCommandHandler : ICreateNewsCommandHandler
    {
        private readonly IAggregateRepository<NewsEntity> _newsRepository;

        public CreateNewsCommandHandler(IAggregateRepository<NewsEntity> newsRepository)
        {
            _newsRepository = newsRepository;
        }

        public async Task<CreateNewsCommandResult> Process(CreateNewsCommand command, ClaimsPrincipal? user)
        {
            var entity = new NewsEntity(Guid.NewGuid());

            entity.SetTitle(command.Title);
            entity.SetAlias(command.Alias);
            entity.SetContent(command.Content);

            await _newsRepository.Save(entity);

            return new CreateNewsCommandResult
            {
                Id = entity.Id
            };
        }
    }
}
