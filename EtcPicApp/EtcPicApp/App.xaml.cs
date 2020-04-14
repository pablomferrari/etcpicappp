using System.Threading.Tasks;
using Akavache;
using EtcPicApp.Bootstrap;
using EtcPicApp.Contracts.Services.Data;
using EtcPicApp.Contracts.Services.General;
using EtcPicApp.Data;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Distribute;
using Microsoft.AppCenter.Push;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace EtcPicApp
{
	public partial class App : Application
	{
        static AsbestosSurveyDatabase database;

        public static AsbestosSurveyDatabase Database
        {
            get
            {
                if(database == null)
                {
                    database = new AsbestosSurveyDatabase();
                }
                return database;
            }
        }
		
		public App ()
		{
		    InitializeComponent();
            InitializeApp();
            InitializeNavigation().ConfigureAwait(false);
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("OTcwOUAzMTM2MmUzMjJlMzBCRkpoaGVKelJVTGxXNUxsNG16VnFWamIvUmp2YitSVi9hM0VuVDdGQy9JPQ==");
		    BlobCache.ApplicationName = "ETC_Pic_App";
			AppCenter.Start("android=0a4e5c88-3ad2-405b-8ace-b3e6ce240acc;ios=ab14d4ae-8b58-4b34-8480-2031ab6da8ff", 
                typeof(Analytics), typeof(Crashes), typeof(Distribute), typeof(Push));
        }

	    private async Task InitializeNavigation()
	    {
	        var navigationService = AppContainer.Resolve<INavigationService>();
	        await navigationService.InitializeAsync();
        }

	    private void InitializeApp()
	    {
	        AppContainer.RegisterDependencies();
        }

        protected override void OnStart ()
		{

        }

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
