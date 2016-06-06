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
using AlertDialog = Android.Support.V7.App.AlertDialog;
using Android.Support.Design;
using Android.Support.Design.Widget;
using Android.Support.V4.App;

namespace Petrolhead
{
    [Activity(Label = "Vehicle Info", Theme = "@style/AppTheme")]
    public class VehicleInfoPage : AppCompatActivity
    {
        VehicleViewModel vehicle;
        TextView description;
        TextView isOverBudget;

        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.VehicleDetails);

            Toolbar toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            vehicle = CoreApp.Current.VehicleManager.SelectedVehicle;
            
            if (vehicle == null)
            {
                AlertDialog.Builder builder = new AlertDialog.Builder(this)
                    .SetTitle("Vehicle Data Error")
                    .SetMessage("Data for your vehicle could not be displayed.")
                    .SetNeutralButton("OK", (s, e) =>
                    {
                        Intent intent = new Intent(this, typeof(MainActivity));
                        StartActivity(intent);
                    });
                builder.Show();
                return;
            }
            else
            {
                description = FindViewById<TextView>(Resource.Id.vDetails_description);
                isOverBudget = FindViewById<TextView>(Resource.Id.bdgtWarn);
                SupportActionBar.Title = vehicle.Vehicle.Name;
                description.Text = vehicle.Vehicle.Description;

                if (vehicle.Vehicle.IsOverBudget)
                    isOverBudget.Visibility = ViewStates.Visible;
                else
                    isOverBudget.Visibility = ViewStates.Invisible;
                
            }
            

           
            
            
            
            // Create your application here
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Messenger.Default.Unregister(this);
        }
    }
}