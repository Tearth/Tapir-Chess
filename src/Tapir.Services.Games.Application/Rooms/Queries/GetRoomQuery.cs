using Dapper;
using Tapir.Core.Commands;
using Tapir.Core.Persistence;
using Tapir.Services.Games.Application.Rooms.Queries.DTOs;

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

        public async Task<RoomDto?> Process(GetRoomQuery request)
        {
            using (var connection = _database.Open())
            {
                return await connection.QueryFirstOrDefaultAsync<RoomDto>("SELECT * FROM Rooms WHERE Id = @Id", new
                {
                    request.Id,
                });
            }
        }
    }
}
