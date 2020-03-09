using System;

namespace EtcPicApp.Models.Sql
{
    public class PhotoCaptionTable : BaseTable
    {
        public long JobId { get; set; }
        public string Caption { get; set; }
        public string Path { get; set; }
    }
}
