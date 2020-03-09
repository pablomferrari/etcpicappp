using System;
using Acr.UserDialogs;
using Android;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Plugin.CurrentActivity;
using Plugin.Permissions;

namespace EtcPicApp.Android
{
    [Activity(Label = "ETC PicApp", Icon = "@drawable/logo", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private readonly string[] Permisssion =
        {
            Manifest.Permission.Camera,
            Manifest.Permission.Internet,
            Manifest.Permission.ReadExternalStorage
        };

        private const int RequestId = 0;
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);
            //Xamarin.Essentials.Platform.Init(this, bundle);
            //RequestPermissions(Permisssion, RequestId);
            CrossCurrentActivity.Current.Init(this, bundle);
            global::Xamarin.Forms.Forms.Init(this, bundle);
            UserDialogs.Init(this);
            LoadApplication(new App());
        }

        //public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        //{
        //    Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

        //    base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        //}

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

