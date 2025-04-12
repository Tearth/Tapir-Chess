using Tapir.Core.Commands;
using Tapir.Core.Persistence;
using Tapir.Core.Types;
using Tapir.Services.News.Domain.News.Entities;

namespace Tapir.Services.News.Application.News.Commands
{
    public class DeleteNewsCommand
    {
        public required Guid Id { get; set; }
    }

    public interface IDeleteNewsCommandHandler : ICommandHandler<DeleteNewsCommand, Unit>
    {

    }

    public class DeleteNewsCommandHandler : IDeleteNewsCommandHandler
    {
        private readonly IAggregateRepository<NewsEntity> _newsRepository;

        public DeleteNewsCommandHandler(IAggregateRepository<NewsEntity> newsRepository)
        {
            _newsRepository = newsRepository;
        }

        public async Task<Unit> Process(DeleteNewsCommand request)
        {
            var entity = await _newsRepository.Load(request.Id);

            entity.Delete();

            await _newsRepository.Save(entity);
            return Unit.Default;
        }
    }
}
