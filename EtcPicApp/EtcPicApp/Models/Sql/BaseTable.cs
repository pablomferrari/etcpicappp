using SQLite;

namespace EtcPicApp.Models.Sql
{
    public abstract class BaseTable
    {
        [PrimaryKey]
        public int Id { get; set; }
    }
}