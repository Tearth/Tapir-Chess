using Dapper;
using System.Security.Claims;
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

        public async Task<PagedResult<PlayerDto>> Process(GetPlayerListQuery query, ClaimsPrincipal? user)
        {
            using (var connection = _database.Open())
            {
                var totalCount = await connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM Players");
                var players = await connection.QueryAsync<PlayerDto>("SELECT * FROM Players ORDER BY CreatedAt DESC LIMIT @Limit OFFSET @Offset", new
                {
                    Limit = query.PageSize,
                    Offset = (query.PageNumber - 1) * query.PageSize,
                });

                return new PagedResult<PlayerDto>
                {
                    Items = players.ToList(),
                    PageNumber = query.PageNumber,
                    PageSize = query.PageSize,
                    TotalCount = totalCount
                };
            }
        }
    }
}
