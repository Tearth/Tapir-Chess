using Dapper;
using System.Security.Claims;
using Tapir.Core.Commands;
using Tapir.Core.Persistence;
using Tapir.Services.Players.Application.Players.Queries.DTOs;

namespace Tapir.Services.Players.Application.Players.Queries
{
    public class GetPlayerQuery
    {
        public Guid Id { get; set; }
    }


    public interface IGetPlayerQueryHandler : ICommandHandler<GetPlayerQuery, PlayerDto?>
    {

    }

    public class GetPlayerQueryHandler : IGetPlayerQueryHandler
    {
        private readonly IDatabaseConnection _database;

        public GetPlayerQueryHandler(IDatabaseConnection database)
        {
            _database = database;
        }

        public async Task<PlayerDto?> Process(GetPlayerQuery query, ClaimsPrincipal? user)
        {
            using (var connection = await _database.Open())
            {
                return await connection.QueryFirstOrDefaultAsync<PlayerDto>("SELECT * FROM Players WHERE Id = @Id", new
                {
                    query.Id,
                });
            }
        }
    }
}
