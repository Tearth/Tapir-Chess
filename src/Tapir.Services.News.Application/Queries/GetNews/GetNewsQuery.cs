using MediatR;
using Tapir.Core.Queries;
using Tapir.Services.News.Application.Models;

namespace Tapir.Services.News.Application.Queries.GetNews
{
    public class GetNewsQuery : IRequest<QueryResult<List<NewsModel>>>
    {

    }
}
