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
            var news = new NewsEntity();

            news.SetTitle(command.Title);
            news.SetAlias(command.Alias);
            news.SetContent(command.Content);

            await _newsRepository.Save(news);
            return Unit.Default;
        }
    }
}
