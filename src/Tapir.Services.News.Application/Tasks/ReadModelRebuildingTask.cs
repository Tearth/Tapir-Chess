using Dapper;
using Microsoft.Extensions.Logging;
using Tapir.Core.Events;
using Tapir.Core.Persistence;
using Tapir.Core.Scheduler;

namespace Tapir.Services.News.Application.Tasks
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
                    await connection.ExecuteAsync($"DROP TABLE IF EXISTS News_Rebuild", null, transaction);
                    await connection.ExecuteAsync($"CREATE TABLE IF NOT EXISTS News_Rebuild (LIKE News INCLUDING ALL)", null, transaction);

                    transaction.Commit();
                }
            }

            await _synchronizer.PublishEvents(true);

            using (var connection = await _database.Open())
            {
                using (var transaction = connection.BeginTransaction())
                {
                    await connection.ExecuteAsync($"ALTER TABLE News RENAME TO News_{DateTime.UtcNow:yyyyMMddHHmmss}", null, transaction);
                    await connection.ExecuteAsync($"ALTER TABLE News_Rebuild RENAME TO News", null, transaction);

                    transaction.Commit();
                }
            }
        }
    }
}
