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
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Messenger = GalaSoft.MvvmLight.Messaging.Messenger;
using AlertDialog = Android.Support.V7.App.AlertDialog;
using Petrolhead.ViewModels;
using GalaSoft.MvvmLight.Messaging;

namespace Petrolhead
{
    [Activity(Label = "VehicleEditorActivity", Theme = "@style/AppTheme")]
    public class VehicleCreatorActivity : AppCompatActivity
    {
        VehicleViewModel vehicle = new VehicleViewModel(new Models.Vehicle());
        EditText nameEntry;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.VehicleEditor);

            Toolbar toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "New Vehicle";
            Toolbar toolbarBottom = FindViewById<Toolbar>(Resource.Id.toolbar_bottom);
            toolbarBottom.Title = "Tools";
            // Create your application here
            nameEntry = FindViewById<EditText>(Resource.Id.nameEntry);
            nameEntry.TextChanged += OnNameChanged;
        }

        private void OnNameChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            var etSender = sender as EditText;

            if (etSender != null)
            {
                if (string.IsNullOrWhiteSpace(etSender.Text))
                {
                    SupportActionBar.Title = "New Vehicle";
                }
                else
                {
                    SupportActionBar.Title = etSender.Text;
                    vehicle.Vehicle.Name = etSender.Text;
                }


            }

        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.vEditorMenu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.TitleFormatted.ToString() == "Save")
            {
                Intent intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
                Messenger.Default.Send(new NotificationMessage<VehicleViewModel>(vehicle, "Add-Vehicle"));
            }
            else if (item.TitleFormatted.ToString() == "Cancel")
            {
                AlertDialog.Builder builder = new AlertDialog.Builder(this)
                    .SetTitle("Are you sure?")
                    .SetMessage("Are you sure you want to cancel creating a new vehicle?")
                    .SetPositiveButton("I'm sure!", (s, e) =>
                    {
                        Intent intent = new Intent(this, typeof(MainActivity));
                        StartActivity(intent);
                    })
                    .SetNegativeButton("No, I'm not!", (s, e) =>
                    {
                    });
                builder.Show();
                
            }
            else
            {

            }
            return base.OnOptionsItemSelected(item);
        }

        protected override void OnStart()
        {
            base.OnStart();
            vehicle = new VehicleViewModel(new Models.Vehicle());
        }
    }
}