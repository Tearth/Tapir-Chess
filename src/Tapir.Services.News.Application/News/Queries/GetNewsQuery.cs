using Dapper;
using MediatR;
using Tapir.Core.Persistence;
using Tapir.Services.News.Application.News.Queries.DTOs;

namespace Tapir.Services.News.Application.News.Queries
{
    public class GetNewsQuery : IRequest<NewsDto>
    {
        public Guid Id { get; set; }
    }

    public class GetNewsQueryHandler : IRequestHandler<GetNewsQuery, NewsDto>
    {
        private readonly IDatabaseConnection _database;

        public GetNewsQueryHandler(IDatabaseConnection database)
        {
            _database = database;
        }

        public async Task<NewsDto?> Handle(GetNewsQuery request, CancellationToken cancellationToken)
        {
            using (var connection = _database.Open())
            {
                return await connection.QueryFirstOrDefaultAsync<NewsDto>("SELECT * FROM News WHERE Id = @Id AND Deleted = false", new
                {
                    request.Id,
                });
            }
        }
    }
}
