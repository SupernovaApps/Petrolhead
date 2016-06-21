using LocalNotifications.Plugin;
using Petrolhead.Helpers;
using Petrolhead.Services;
using Petrolhead.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Xamarin.Forms;

namespace Petrolhead
{
    public delegate void OnVehicleCreated(ref VehicleViewModel vehicle);
    public class App : Application
    {
        public App()
        {


            // The root page of your application
            MainPage = new Views.LoginPage();
        }

        public OnVehicleCreated OnVehicleCreated
        {
            get;
            set;
        }
        protected override void OnStart()
        {
           
            
        }

        protected override async void OnSleep()
        {
            Settings.LastSync = DateTime.Now.AddDays(-4);
            await DataStore.Current.SyncVehiclesAsync();
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
