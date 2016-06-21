using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Petrolhead.Services;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Petrolhead.Droid.Services;

namespace Petrolhead.Droid
{
    [Activity(Label = "Petrolhead", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
    

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            CurrentPlatform.Init();
            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());
            DataStore.Current.Authenticator = Authenticator.Current;
            
        }
    }
}

