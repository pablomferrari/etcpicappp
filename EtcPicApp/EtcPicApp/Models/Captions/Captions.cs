namespace EtcPicApp.Models.Captions
{
    public class Captions
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public string Caption { get; set; }
        public bool IsRequired { get; set; }
    }
}
