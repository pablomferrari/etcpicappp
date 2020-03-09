using System.Collections.Generic;
using System.Linq;
using EtcPicApp.Models.Captions;
using EtcPicApp.Models.PhotoCaption;
using EtcPicApp.Models.Sql;

namespace EtcPicApp.Extensions
{
    public static class ObjectExtensions
    {

        public static IEnumerable<CaptionsTable> ToTable(this IEnumerable<Captions> list)
        {
            return list.Select(x => new CaptionsTable 
            {
                ServiceId = x.ServiceId,
                Caption = x.Caption,
                IsRequired = x.IsRequired
            });
        }

        public static PhotoCaptionTable ToTable(this PhotoStreamCaption model)
        {
            return new PhotoCaptionTable
            {
               Id = model.Id,
               Caption = model.Caption,
               JobId = model.JobId,
               Path = model.Path
            };
        }


        public static PhotoCaption ToModel(this PhotoCaptionTable table)
        {
            return new PhotoCaption
            {
                Id = table.Id,
                JobId = table.JobId,
                Caption = table.Caption,
                Path = table.Path              

            };
        }
    }
}
