﻿using Npgsql;
using System.Data;
using Tapir.Core.Persistence;

namespace Tapir.Providers.Database.PostgreSQL.Persistence
{
    public class DatabaseConnection : IDatabaseConnection
    {
        private readonly string _connectionString;

        public DatabaseConnection(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IDbConnection> Open()
        {
            var connection = new NpgsqlConnection(_connectionString);
           
            await connection.OpenAsync();
            return connection;
        }
    }
}
