﻿using Dapper;
using System.Security.Claims;
using Tapir.Core.Commands;
using Tapir.Core.Persistence;
using Tapir.Services.News.Application.News.Queries.DTOs;

namespace Tapir.Services.News.Application.News.Queries
{
    public class GetNewsQuery
    {
        public Guid Id { get; set; }
    }

    public interface IGetNewsQueryHandler : ICommandHandler<GetNewsQuery, NewsDto?>
    {

    }

    public class GetNewsQueryHandler : IGetNewsQueryHandler
    {
        private readonly IDatabaseConnection _database;

        public GetNewsQueryHandler(IDatabaseConnection database)
        {
            _database = database;
        }

        public async Task<NewsDto?> Process(GetNewsQuery query, ClaimsPrincipal? user)
        {
            using (var connection = await _database.Open())
            {
                return await connection.QueryFirstOrDefaultAsync<NewsDto>("SELECT * FROM News WHERE Id = @Id AND Deleted = false", new
                {
                    query.Id,
                });
            }
        }
    }
}
