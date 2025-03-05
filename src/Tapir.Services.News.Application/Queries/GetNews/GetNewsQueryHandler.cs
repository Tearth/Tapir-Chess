using MediatR;
using Tapir.Common.Infrastructure.Queries;
using Tapir.Services.News.Application.Models;

namespace Tapir.Services.News.Application.Queries.GetNews
{
    public class GetNewsQueryHandler : IRequestHandler<GetNewsQuery, QueryResult<List<NewsModel>>>
    {
        public async Task<QueryResult<List<NewsModel>>> Handle(GetNewsQuery request, CancellationToken cancellationToken)
        {
            return QueryResult<List<NewsModel>>.Success(
            [
                new NewsModel { Guid = Guid.NewGuid() },
                new NewsModel { Guid = Guid.NewGuid() },
                new NewsModel { Guid = Guid.NewGuid() },
            ]);
        }
    }
}
