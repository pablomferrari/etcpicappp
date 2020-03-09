using EtcPicApp.Models.Sql;

namespace EtcPicApp.Models.PhotoCaption
{
    public class PhotoCaption
    {
        public int Id { get; set; }
        public long JobId { get; set; }
        public string Source { get; set; }
        public string Caption { get; set; }
        public string Path { get; set; }

        public static PhotoCaptionTable ToTable(PhotoCaption photo)
        {
            return new PhotoCaptionTable
            {
                Id = photo.Id,
                JobId = photo.JobId,
                Caption = photo.Caption,
                Path = photo.Path
            };
        }
    }
}
