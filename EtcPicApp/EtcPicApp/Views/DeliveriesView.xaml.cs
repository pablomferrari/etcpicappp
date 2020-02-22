using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EtcPicApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeliveriesView : ContentPage
    {
        public DeliveriesView()
        {
            InitializeComponent();
            if (Device.Idiom == TargetIdiom.Tablet)
            {
                SaveButton.HeightRequest = 75;
                // Sample.RowHeight = 65;
            }
        }
    }
}