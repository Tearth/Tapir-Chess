using Dapper;
using Microsoft.Extensions.Logging;
using Tapir.Core.Events;
using Tapir.Core.Persistence;
using Tapir.Core.Scheduler;

namespace Tapir.Services.Games.Application.Tasks
{
    public class ReadModelRebuildingTask : ITask
    {
        private readonly IDomainEventSynchronizer _synchronizer;
        private readonly IDatabaseConnection _database;

        public ReadModelRebuildingTask(IDomainEventSynchronizer? synchronizer = null, IDatabaseConnection? database = null)
        {
            _synchronizer = synchronizer!;
            _database = database!;
        }

        public async Task Run()
        {
            using (var connection = await _database.Open())
            {
                using (var transaction = connection.BeginTransaction())
                {
                    await connection.ExecuteAsync($"DROP TABLE IF EXISTS Games_Rebuild", null, transaction);
                    await connection.ExecuteAsync($"DROP TABLE IF EXISTS Rooms_Rebuild", null, transaction);
                    await connection.ExecuteAsync($"CREATE TABLE IF NOT EXISTS Games_Rebuild (LIKE Games INCLUDING ALL)", null, transaction);
                    await connection.ExecuteAsync($"CREATE TABLE IF NOT EXISTS Rooms_Rebuild (LIKE Rooms INCLUDING ALL)", null, transaction);

                    transaction.Commit();
                }
            }

            await _synchronizer.PublishEvents(true);

            using (var connection = await _database.Open())
            {
                using (var transaction = connection.BeginTransaction())
                {
                    await connection.ExecuteAsync($"ALTER TABLE Games RENAME TO Games_{DateTime.UtcNow:yyyyMMddHHmmss}", null, transaction);
                    await connection.ExecuteAsync($"ALTER TABLE Rooms RENAME TO Rooms_{DateTime.UtcNow:yyyyMMddHHmmss}", null, transaction);
                    await connection.ExecuteAsync($"ALTER TABLE Games_Rebuild RENAME TO Games", null, transaction);
                    await connection.ExecuteAsync($"ALTER TABLE Rooms_Rebuild RENAME TO Rooms", null, transaction);

                    transaction.Commit();
                }
            }
        }
    }
}
