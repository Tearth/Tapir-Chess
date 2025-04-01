using MediatR;
using Tapir.Core.Persistence;
using Tapir.Services.News.Domain.News.Entities;

namespace Tapir.Services.News.Application.News.Commands
{
    public class UpdateNewsCommand : IRequest<Unit>
    {
        public required Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Alias { get; set; }
        public string? Content { get; set; }
    }

    public class UpdateNewsCommandHandler : IRequestHandler<UpdateNewsCommand, Unit>
    {
        private readonly IAggregateRepository<NewsEntity> _newsRepository;

        public UpdateNewsCommandHandler(IAggregateRepository<NewsEntity> newsRepository)
        {
            _newsRepository = newsRepository;
        }

        public async Task<Unit> Handle(UpdateNewsCommand request, CancellationToken cancellationToken)
        {
            var aggregate = await _newsRepository.Load(request.Id);

            if (request.Title != null)
            {
                aggregate.SetTitle(request.Title);
            }

            if (request.Alias != null)
            {
                aggregate.SetAlias(request.Alias);
            }

            if (request.Content != null)
            {
                aggregate.SetContent(request.Content);
            }

            await _newsRepository.Save(aggregate);
            return Unit.Value;
        }
    }
}
