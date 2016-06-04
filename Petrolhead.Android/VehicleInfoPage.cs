using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;
using Messenger = GalaSoft.MvvmLight.Messaging.Messenger;
using GalaSoft.MvvmLight.Messaging;
using Petrolhead.ViewModels;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.Design;
using Android.Support.Design.Widget;
using Android.Support.V4.App;

namespace Petrolhead
{
    [Activity(Label = "Vehicle Info", Theme = "@style/AppTheme")]
    public class VehicleInfoPage : AppCompatActivity
    {
        VehicleViewModel vehicle;


        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.VehicleDetails);

            Toolbar toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            Title = CoreApp.Current.VehicleManager.SelectedVehicle.Vehicle.Name;

            

            

           
            
            
            
            // Create your application here
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Messenger.Default.Unregister(this);
        }
    }
}