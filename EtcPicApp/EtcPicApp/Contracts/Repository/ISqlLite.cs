
using SQLite;

namespace EtcPicApp.Contracts.Repository
{
    public interface ISqlLite
    {
        SQLiteAsyncConnection GetConnectionAsync();
    }
}
