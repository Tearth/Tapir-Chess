using MediatR;
using Tapir.Core.Persistence;
using Tapir.Services.News.Domain.News.Entities;

namespace Tapir.Services.News.Application.News.Commands
{
    public class DeleteNewsCommand : IRequest<Unit>
    {
        public required Guid Id { get; set; }
    }

    public class DeleteNewsCommandHandler : IRequestHandler<DeleteNewsCommand, Unit>
    {
        private readonly IAggregateRepository<NewsEntity> _newsRepository;

        public DeleteNewsCommandHandler(IAggregateRepository<NewsEntity> newsRepository)
        {
            _newsRepository = newsRepository;
        }

        public async Task<Unit> Handle(DeleteNewsCommand request, CancellationToken cancellationToken)
        {
            var aggregate = await _newsRepository.Load(request.Id);

            aggregate.Delete();

            await _newsRepository.Save(aggregate);
            return Unit.Value;
        }
    }
}
