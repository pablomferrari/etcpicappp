using System;

namespace EtcPicApp.Models.Sql
{
    public class CaptionsTable : BaseTable
    {
        public int ServiceId { get; set; }
        public string Caption { get; set; }
        public bool IsRequired { get; set; }

        public static Captions.Captions ToModel(CaptionsTable table)
        {
            return new Captions.Captions
            {
                Id = table.Id,
                ServiceId = table.ServiceId,
                Caption = table.Caption,
                IsRequired = table.IsRequired

            };
        }
    }
}
