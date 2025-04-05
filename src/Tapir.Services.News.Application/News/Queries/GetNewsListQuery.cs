using Dapper;
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

        public async Task<PagedResult<NewsDto>> Process(GetNewsListQuery request)
        {
            using (var connection = _database.Open())
            {
                var totalCount = await connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM News WHERE Deleted = false");
                var items = await connection.QueryAsync<NewsDto>("SELECT * FROM News WHERE Deleted = false ORDER BY CreatedAt DESC LIMIT @Limit OFFSET @Offset", new
                {
                    Limit = request.PageSize,
                    Offset = (request.PageNumber - 1) * request.PageSize,
                });

                return new PagedResult<NewsDto>
                {
                    Items = items.ToList(),
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize,
                    TotalCount = totalCount
                };
            }
        }
    }
}
