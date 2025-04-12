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

    public interface ICreateNewsCommandHandler : ICommandHandler<CreateNewsCommand, Unit>
    {

    }

    public class CreateNewsCommandHandler : ICreateNewsCommandHandler
    {
        private readonly IAggregateRepository<NewsEntity> _newsRepository;

        public CreateNewsCommandHandler(IAggregateRepository<NewsEntity> newsRepository)
        {
            _newsRepository = newsRepository;
        }

        public async Task<Unit> Process(CreateNewsCommand command)
        {
            var entity = new NewsEntity(Guid.NewGuid());

            entity.SetTitle(command.Title);
            entity.SetAlias(command.Alias);
            entity.SetContent(command.Content);

            await _newsRepository.Save(entity);
            return Unit.Default;
        }
    }
}
