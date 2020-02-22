using System.IO;
using EtcPicApp.Android;
using EtcPicApp.Constants;
using EtcPicApp.Data;
using SQLite;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLite_Android))]
namespace EtcPicApp.Android
{
    public class SQLite_Android : ISQLite
    {
        public SQLite_Android()
        {
        }

        #region ISQLite implementation

        public SQLiteAsyncConnection GetConnection()
        {
            var path = Path.Combine(System.Environment.
                GetFolderPath(System.Environment.
                    SpecialFolder.Personal), DatabaseConstants.DbName);

            return new SQLiteAsyncConnection(path);
        }

        #endregion
    }
}