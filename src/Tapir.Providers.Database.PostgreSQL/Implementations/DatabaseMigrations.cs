
using DbUp;

namespace Tapir.Providers.Database.PostgreSQL.Implementations
{
    public class DatabaseMigrations
    {
        private readonly Configuration _configuration;

        public DatabaseMigrations(Configuration configuration)
        {
            _configuration = configuration;
        }

        public void Execute()
        {
            var builder = DeployChanges.To.PostgresqlDatabase(_configuration.ConnectionString).JournalToPostgresqlTable("public", "schema_version");
            var migrator = builder.WithScriptsEmbeddedInAssembly(_configuration.MigrationsAssembly).Build();
            var result = migrator.PerformUpgrade();

            if (!result.Successful)
            {
                throw result.Error;
            }
        }
    }
}
