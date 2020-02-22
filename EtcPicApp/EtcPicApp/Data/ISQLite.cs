using SQLite;

namespace EtcPicApp.Data
{
    public interface ISQLite
    {
        SQLiteAsyncConnection GetConnection();
    }
}
