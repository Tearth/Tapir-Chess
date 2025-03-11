using System.Reflection;

namespace Tapir.Providers.Database.PostgreSQL
{
    public class Configuration
    {
        public string ConnectionString { get; set; }
        public Assembly MigrationsAssembly { get; set; }
    }
}
