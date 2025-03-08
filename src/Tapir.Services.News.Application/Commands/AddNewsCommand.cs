using MediatR;
using Tapir.Core.Interfaces;
using Tapir.Services.News.Domain.News.Entities;

namespace Tapir.Services.News.Application.Commands
{
    public class AddNewsCommand : IRequest<Unit>
    {

    }

    public class AddNewsCommandHandler : IRequestHandler<AddNewsCommand, Unit>
    {
        private readonly IAggregateRepository<NewsEntity> _newsRepository;

        public AddNewsCommandHandler(IAggregateRepository<NewsEntity> newsRepository)
        {
            _newsRepository = newsRepository;
        }

        public async Task<Unit> Handle(AddNewsCommand request, CancellationToken cancellationToken)
        {
            var news = new NewsEntity();
            
            await _newsRepository.Save(news);
            return Unit.Value;
        }
    }
}
