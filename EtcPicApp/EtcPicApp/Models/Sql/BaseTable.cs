using SQLite;

namespace EtcPicApp.Models.Sql
{
    public abstract class BaseTable
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
    }
}