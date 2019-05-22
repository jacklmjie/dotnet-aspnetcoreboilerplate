using System.Data;

namespace Core.Repository
{
    public interface IDbRepository
    {
        IDbConnection Connection { get; }
    }
}