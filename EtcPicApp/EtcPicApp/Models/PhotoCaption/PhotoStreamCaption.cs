using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace EtcPicApp.Models.PhotoCaption
{
    public class PhotoStreamCaption : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public long JobId { get; set; }
        public Stream Stream { get; set; }
        private string _caption;
        public string Caption
        {
            get => _caption;
            set
            {
                _caption = value;
                OnPropertyChanged();
            }
        }
        public ImageSource Source => ImageSource.FromStream(() => { return this.Stream; });
        public string Path { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
