using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EtcPicApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RoomsView : ContentPage
    {
        public ObservableCollection<string> Items { get; set; }

        public RoomsView()
        {
            InitializeComponent();
            // ListView.ItemTapped += ItemTapped;
        }

    }
}
