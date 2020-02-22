using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EtcPicApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ETLNavigationPage : NavigationPage
	{
		public ETLNavigationPage ()
		{
			InitializeComponent ();
		}

	    public ETLNavigationPage(Page root) : base(root)
	    {
	        InitializeComponent();
	    }
    }
}