using MediatR;
using Tapir.Core.Persistence;
using Tapir.Services.News.Domain.News.Entities;

namespace Tapir.Services.News.Application.News.Commands
{
    public class CreateNewsCommand : IRequest<Unit>
    {

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
            await _newsRepository.Save(new NewsEntity());
            return Unit.Value;
        }
    }
}
