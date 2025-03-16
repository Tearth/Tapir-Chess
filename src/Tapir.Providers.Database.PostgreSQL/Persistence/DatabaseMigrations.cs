
using DbUp;
using Microsoft.Extensions.Hosting;

namespace Tapir.Providers.Database.PostgreSQL.Persistence
{
    public class DatabaseMigrations : BackgroundService
    {
        private readonly Configuration _configuration;

        public DatabaseMigrations(Configuration configuration)
        {
            _configuration = configuration;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var builder = DeployChanges.To.PostgresqlDatabase(_configuration.ConnectionString).JournalToPostgresqlTable("public", "schema_version");
            var migrator = builder.WithScriptsEmbeddedInAssembly(_configuration.MigrationsAssembly).Build();
            var result = migrator.PerformUpgrade();

            if (!result.Successful)
            {
                throw result.Error;
            }

            return Task.CompletedTask;
        }
    }
}
