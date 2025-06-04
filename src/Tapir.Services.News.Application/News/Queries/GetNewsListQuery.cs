using Dapper;
using System.Security.Claims;
using Tapir.Core.Commands;
using Tapir.Core.Persistence;
using Tapir.Core.Queries;
using Tapir.Services.News.Application.News.Queries.DTOs;

namespace Tapir.Services.News.Application.News.Queries
{
    public class GetNewsListQuery
    {
        public required int PageNumber { get; set; }
        public required int PageSize { get; set; }
    }

    public interface IGetNewsListQueryHandler : ICommandHandler<GetNewsListQuery, PagedResult<NewsDto>>
    {

    }

    public class GetNewsListQueryHandler : IGetNewsListQueryHandler
    {
        private readonly IDatabaseConnection _database;

        public GetNewsListQueryHandler(IDatabaseConnection database)
        {
            _database = database;
        }

        public async Task<PagedResult<NewsDto>> Process(GetNewsListQuery query, ClaimsPrincipal? user)
        {
            using (var connection = await _database.Open())
            {
                var totalCount = await connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM News WHERE Deleted = false");
                var news = await connection.QueryAsync<NewsDto>("SELECT * FROM News WHERE Deleted = false ORDER BY CreatedAt DESC LIMIT @Limit OFFSET @Offset", new
                {
                    Limit = query.PageSize,
                    Offset = (query.PageNumber - 1) * query.PageSize,
                });

                return new PagedResult<NewsDto>
                {
                    Items = news.ToList(),
                    PageNumber = query.PageNumber,
                    PageSize = query.PageSize,
                    TotalCount = totalCount
                };
            }
        }
    }
}
