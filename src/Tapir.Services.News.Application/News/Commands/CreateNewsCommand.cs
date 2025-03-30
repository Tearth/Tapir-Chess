using MediatR;
using Tapir.Core.Persistence;
using Tapir.Services.News.Domain.News.Entities;

namespace Tapir.Services.News.Application.News.Commands
{
    public class CreateNewsCommand : IRequest<Unit>
    {
        public required string Title { get; set; }
        public required string Alias { get; set; }
        public required string Content { get; set; }
    }

    public class CreateNewsCommandHandler : IRequestHandler<CreateNewsCommand, Unit>
    {
        private readonly IAggregateRepository<NewsEntity> _newsRepository;

        public CreateNewsCommandHandler(IAggregateRepository<NewsEntity> newsRepository)
        {
            _newsRepository = newsRepository;
        }

        public async Task<Unit> Handle(CreateNewsCommand request, CancellationToken cancellationToken)
        {
            var news = new NewsEntity();
            news.SetTitle(request.Title);
            news.SetAlias(request.Alias);
            news.SetContent(request.Content);

            await _newsRepository.Save(news);
            return Unit.Value;
        }
    }
}
