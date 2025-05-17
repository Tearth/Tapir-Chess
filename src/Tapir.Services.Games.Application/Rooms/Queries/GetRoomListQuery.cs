using Dapper;
using Tapir.Core.Commands;
using Tapir.Core.Persistence;
using Tapir.Core.Queries;
using Tapir.Services.Games.Application.Rooms.Queries.DTOs;
using Tapir.Services.Games.Domain.Rooms.Entities;

namespace Tapir.Services.Games.Application.Rooms.Queries
{
    public class GetRoomListQuery
    {
        public required int PageNumber { get; set; }
        public required int PageSize { get; set; }
    }


    public interface IGetRoomListQueryHandler : ICommandHandler<GetRoomListQuery, PagedResult<RoomDto>>
    {

    }

    public class GetRoomListQueryHandler : IGetRoomListQueryHandler
    {
        private readonly IDatabaseConnection _database;

        public GetRoomListQueryHandler(IDatabaseConnection database)
        {
            _database = database;
        }

        public async Task<PagedResult<RoomDto>> Process(GetRoomListQuery request)
        {
            using (var connection = _database.Open())
            {
                var totalCount = await connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM Rooms WHERE Status = @Status", new
                {
                    Status = RoomStatus.Open
                });

                var rooms = await connection.QueryAsync<RoomDto>("SELECT * FROM Rooms WHERE Status = @Status ORDER BY CreatedAt DESC LIMIT @Limit OFFSET @Offset", new
                {
                    Status = RoomStatus.Open,
                    Limit = request.PageSize,
                    Offset = (request.PageNumber - 1) * request.PageSize,
                });

                return new PagedResult<RoomDto>
                {
                    Items = rooms.ToList(),
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize,
                    TotalCount = totalCount
                };
            }
        }
    }
}
