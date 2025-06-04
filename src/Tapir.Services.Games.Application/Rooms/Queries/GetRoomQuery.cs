using Dapper;
using System.Security.Claims;
using Tapir.Core.Commands;
using Tapir.Core.Persistence;
using Tapir.Services.Games.Application.Rooms.Queries.DTOs;
using Tapir.Services.Games.Domain.Rooms.Entities;

namespace Tapir.Services.Games.Application.Rooms.Queries
{
    public class GetRoomQuery
    {
        public Guid Id { get; set; }
    }


    public interface IGetRoomQueryHandler : ICommandHandler<GetRoomQuery, RoomDto?>
    {

    }

    public class GetRoomQueryHandler : IGetRoomQueryHandler
    {
        private readonly IDatabaseConnection _database;

        public GetRoomQueryHandler(IDatabaseConnection database)
        {
            _database = database;
        }

        public async Task<RoomDto?> Process(GetRoomQuery query, ClaimsPrincipal? user)
        {
            using (var connection = await _database.Open())
            {
                return await connection.QueryFirstOrDefaultAsync<RoomDto>("SELECT * FROM Rooms WHERE Id = @Id AND Status = @Status", new
                {
                    query.Id,
                    Status = RoomStatus.Open
                });
            }
        }
    }
}
