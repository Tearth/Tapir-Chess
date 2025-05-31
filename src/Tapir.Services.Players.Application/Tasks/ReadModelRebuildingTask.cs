using Dapper;
using Microsoft.Extensions.Logging;
using Tapir.Core.Events;
using Tapir.Core.Persistence;
using Tapir.Core.Scheduler;

namespace Tapir.Services.Players.Application.Tasks
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
            using (var connection = _database.Open())
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    await connection.ExecuteAsync($"DROP TABLE IF EXISTS Players_Rebuild", null, transaction);
                    await connection.ExecuteAsync($"CREATE TABLE IF NOT EXISTS Players_Rebuild (LIKE Players INCLUDING ALL)", null, transaction);

                    transaction.Commit();
                }
            }

            await _synchronizer.PublishEvents(true);

            using (var connection = _database.Open())
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    await connection.ExecuteAsync($"ALTER TABLE Players RENAME TO Players_{DateTime.UtcNow:yyyyMMddHHmmss}", null, transaction);
                    await connection.ExecuteAsync($"ALTER TABLE Players_Rebuild RENAME TO Players", null, transaction);

                    transaction.Commit();
                }
            }
        }
    }
}
