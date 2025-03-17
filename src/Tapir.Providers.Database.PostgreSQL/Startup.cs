using DbUp;
using Microsoft.Extensions.Hosting;

namespace Tapir.Providers.Database.PostgreSQL
{
    public class Startup : IHostedService
    {
        private readonly Configuration _configuration;

        public Startup(Configuration configuration)
        {
            _configuration = configuration;
        }

        public Task StartAsync(CancellationToken cancellationToken)
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

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
