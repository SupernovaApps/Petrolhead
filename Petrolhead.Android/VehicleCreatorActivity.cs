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
using System.Threading.Tasks;

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

            Button yofm = FindViewById<Button>(Resource.Id.setManufactureYear);
            Button yofp = FindViewById<Button>(Resource.Id.setPurchaseYear);

            yofp.Click += (s, e) =>
            {
                DateTime date = DateTime.Today;
                if (vehicle.Vehicle.YearOfPurchase != null)
                    date = DateTime.SpecifyKind(new DateTime(vehicle.Vehicle.YearOfPurchase.Value.Year, vehicle.Vehicle.YearOfPurchase.Value.Month, vehicle.Vehicle.YearOfPurchase.Value.Day), DateTimeKind.Local);
                DatePickerFragment fragment = DatePickerFragment.NewInstance(delegate (DateTime dateTime)
                {
                    DateTime dt = new DateTime(dateTime.Year, 1, dateTime.Day).AddMonths(-1);
                    vehicle.Vehicle.YearOfPurchase = dt;
                }, date);
                fragment.Show(FragmentManager, "Set Purchase Year");
            };

            yofm.Click += (s, e) =>
            {
                DateTime date = DateTime.Today;

                if (vehicle.Vehicle.YearOfManufacture != null)
                {
                    DateTimeOffset offset = (DateTimeOffset)vehicle.Vehicle.YearOfManufacture;
                    date = DateTime.SpecifyKind(new DateTime(offset.Year, offset.Month, offset.Day), DateTimeKind.Local);
                }

                DatePickerFragment fragment = DatePickerFragment.NewInstance(delegate (DateTime dateTime)
                {
                    DateTime dt = new DateTime(dateTime.Year, 1, dateTime.Day).AddMonths(-1);
                    vehicle.Vehicle.YearOfManufacture = dt;
                }, date);
                fragment.Show(FragmentManager, "Set Manufacture Year");
            };


            EditText descriptionEntry = FindViewById<EditText>(Resource.Id.descriptionEntry);
            descriptionEntry.TextChanged += (s, e) =>
            {
                vehicle.Vehicle.Description = descriptionEntry.Text;
            };

            EditText manufacturerEntry = FindViewById<EditText>(Resource.Id.manufacturerEntry);
            manufacturerEntry.TextChanged += (s, e) =>
            {
                vehicle.Vehicle.Manufacturer = manufacturerEntry.Text;
            };

            EditText modelEntry = FindViewById<EditText>(Resource.Id.modelEntry);
            modelEntry.TextChanged += (s, e) =>
            {
                vehicle.Vehicle.Model = modelEntry.Text;
            };

            Button setWarrantDate = FindViewById<Button>(Resource.Id.setWarrantDate);
            setWarrantDate.Click += (s, e) =>
            {
                DateTime date = DateTime.Today;
                if (vehicle.Vehicle.NextWarrantDate != null)
                {
                    DateTimeOffset offset = (DateTimeOffset)vehicle.Vehicle.NextWarrantDate;
                    date = DateTime.SpecifyKind(new DateTime(offset.Year, offset.Month, offset.Day), DateTimeKind.Local);
                }
                DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime dateTime)
                {
                    vehicle.Vehicle.NextWarrantDate = dateTime.AddMonths(-1);

                }, date);
                frag.Show(FragmentManager, "Warrant");
            };

            Button setRegoDate = FindViewById<Button>(Resource.Id.setRegoDate);
            setRegoDate.Click += (s, e) =>
            {
                DateTime date = DateTime.Today;
                if (vehicle.Vehicle.NextRegoRenewal != null)
                {
                    DateTimeOffset offset = (DateTimeOffset)vehicle.Vehicle.NextRegoRenewal;
                    date = DateTime.SpecifyKind(new DateTime(offset.Year, offset.Month, offset.Day), DateTimeKind.Local);
                }

                DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime dateTime)
                {
                    vehicle.Vehicle.NextRegoRenewal = dateTime.AddMonths(-1);

                }, date);
                frag.Show(FragmentManager, "Rego");
            };
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

        private async void Save()
        {
            if (CoreApp.Current.VehicleValidator.Validate(vehicle))
            {
                Intent intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
                await Task.Delay(1500);
                Messenger.Default.Send(new NotificationMessage<VehicleViewModel>(vehicle, "Add-Vehicle"));
            }
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.menu_save)
            {
                Save();
            }
            else if (item.ItemId == Resource.Id.menu_cancel)
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