using System.Data;

namespace Tapir.Core.Persistence
{
    public interface IDatabaseConnection
    {
        Task<IDbConnection> Open();
    }
}
