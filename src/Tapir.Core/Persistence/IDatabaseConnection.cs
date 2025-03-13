using System.Data;

namespace Tapir.Core.Persistence
{
    public interface IDatabaseConnection
    {
        IDbConnection Open();
    }
}
