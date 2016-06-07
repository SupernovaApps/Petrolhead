using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlertDialog = Android.Support.V7.App.AlertDialog;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Petrolhead.ViewModels;
using GalaSoft.MvvmLight.Messaging;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Java.IO;

namespace Petrolhead
{
    public class VehicleWrapper : Java.Lang.Object, ISerializable
    {
        public VehicleWrapper(VehicleViewModel vehicle)
        {
            Vehicle = vehicle;
            
        }

        public VehicleViewModel Vehicle { get; private set; }
    }
    public class VehicleAdapter : BaseAdapter<VehicleViewModel>
    {
        List<VehicleViewModel> vehicles = new List<VehicleViewModel>();
        Activity activity;
        int layoutResourceId;

        private List<VehicleViewModel> Vehicles { get { return vehicles; }
        set { vehicles = value; }
        }

        

        public VehicleAdapter(Activity activity, int layoutResourceId)
        {
            this.activity = activity;
            this.layoutResourceId = layoutResourceId;
            
        }

       

        public override int Count
        {
            get
            {
                return Vehicles.Count;
            }
        }

        private VehicleViewModel selectedVehicle;
        public VehicleViewModel SelectedVehicle
        {
            get
            {
                return selectedVehicle;
            }
            set
            {
                selectedVehicle = value;
                CoreApp.Current.VehicleManager.SelectedVehicle = value;
            }
        }

        public override VehicleViewModel this[int position]
        {
            get
            {
                return Vehicles[position];
            }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var row = convertView;
            var currentItem = this[position];
            CheckBox checkBox;
            ImageView warningView;

            if (row == null)
            {

                var inflater = activity.LayoutInflater;
                row = inflater.Inflate(layoutResourceId, parent, false);

                checkBox = row.FindViewById<CheckBox>(Resource.Id.checkVehicle);
                warningView = row.FindViewById<ImageView>(Resource.Id.warningView);

                currentItem.TotalUpdated += (s, e) =>
                {
                    if (currentItem.Vehicle.IsOverBudget || currentItem.Vehicle.NextRegoRenewal < DateTime.Today || currentItem.Vehicle.NextWarrantDate < DateTime.Today)
                        warningView.Visibility = ViewStates.Visible;
                    else
                        warningView.Visibility = ViewStates.Invisible;

                };

                currentItem.RegistrationOverdue += (s, e) =>
                {
                    if (currentItem.Vehicle.IsOverBudget || currentItem.Vehicle.NextRegoRenewal < DateTime.Today || currentItem.Vehicle.NextWarrantDate < DateTime.Today)
                        warningView.Visibility = ViewStates.Visible;
                    else
                        warningView.Visibility = ViewStates.Invisible;
                };

                currentItem.WarrantOverdue += (s, e) =>
                {
                    if (currentItem.Vehicle.IsOverBudget || currentItem.Vehicle.NextRegoRenewal <= DateTime.Today || currentItem.Vehicle.NextWarrantDate <= DateTime.Today)
                        warningView.Visibility = ViewStates.Visible;
                    else
                        warningView.Visibility = ViewStates.Invisible;
                };

                checkBox.CheckedChange += (s, e) =>
                {
                    var cbSender = s as CheckBox;

                    if (cbSender != null && cbSender.Tag is VehicleWrapper && cbSender.Checked)
                    {
                        this.SelectedVehicle = (cbSender.Tag as VehicleWrapper).Vehicle;
                    }
                };

                if (currentItem.Vehicle.IsOverBudget || currentItem.Vehicle.NextRegoRenewal <= DateTime.Today || currentItem.Vehicle.NextWarrantDate <= DateTime.Today)
                    warningView.Visibility = ViewStates.Visible;
                else
                    warningView.Visibility = ViewStates.Invisible;

            }
            else
                checkBox = row.FindViewById<CheckBox>(Resource.Id.checkVehicle);

                checkBox.Text = currentItem.Vehicle.Name + " " + currentItem.Vehicle.HumanTotal;
                checkBox.Checked = false;
                checkBox.Enabled = true;
                checkBox.Tag = new VehicleWrapper(currentItem);

            return row;
        }

      

        public override long GetItemId(int position)
        {
            return position;
        }

        public void Add(VehicleViewModel vm)
        {
            Vehicles.Add(vm);
            SelectedVehicle = vm;
            NotifyDataSetChanged();
        }

        public void Remove(VehicleViewModel vm)
        {
            Vehicles.Remove(vm);
            SelectedVehicle = vm;
            NotifyDataSetChanged();
        }

        public void Clear()
        {
            Vehicles.Clear();
            NotifyDataSetChanged();
        }

        

    }
}