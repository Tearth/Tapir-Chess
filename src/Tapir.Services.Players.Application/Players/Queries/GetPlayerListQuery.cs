using Dapper;
using Tapir.Core.Commands;
using Tapir.Core.Persistence;
using Tapir.Core.Queries;
using Tapir.Services.Players.Application.Players.Queries.DTOs;

namespace Tapir.Services.Players.Application.Players.Queries
{
    public class GetPlayerListQuery
    {
        public required int PageNumber { get; set; }
        public required int PageSize { get; set; }
    }


    public interface IGetPlayerListQueryHandler : ICommandHandler<GetPlayerListQuery, PagedResult<PlayerDto>>
    {

    }

    public class GetPlayerListQueryHandler : IGetPlayerListQueryHandler
    {
        private readonly IDatabaseConnection _database;

        public GetPlayerListQueryHandler(IDatabaseConnection database)
        {
            _database = database;
        }

        public async Task<PagedResult<PlayerDto>> Process(GetPlayerListQuery request)
        {
            using (var connection = _database.Open())
            {
                var totalCount = await connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM Players");
                var players = await connection.QueryAsync<PlayerDto>("SELECT * FROM Players ORDER BY CreatedAt DESC LIMIT @Limit OFFSET @Offset", new
                {
                    Limit = request.PageSize,
                    Offset = (request.PageNumber - 1) * request.PageSize,
                });

                return new PagedResult<PlayerDto>
                {
                    Items = players.ToList(),
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize,
                    TotalCount = totalCount
                };
            }
        }
    }
}
