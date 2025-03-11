using System.Data;

namespace Tapir.Core.Interfaces
{
    public interface IDatabaseConnection
    {
        IDbConnection Open();
    }
}
