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
using Petrolhead.ViewModels;
using GalaSoft.MvvmLight.Messaging;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Petrolhead
{
    public class VehicleWrapper : Java.Lang.Object
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
        set { vehicles = value; CoreApp.Current.VehicleManager.Clear(); foreach (var vm in value) CoreApp.Current.VehicleManager.Add(vm); }
        }

        public VehicleAdapter(Activity activity, int layoutResourceId)
        {
            this.activity = activity;
            this.layoutResourceId = layoutResourceId;
            vehicles = CoreApp.Current.VehicleManager.ToList();
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
            TextView overBudget;

            if (row == null)
            {
               
                var inflater = activity.LayoutInflater;
                row = inflater.Inflate(layoutResourceId, parent, false);

                checkBox = row.FindViewById<CheckBox>(Resource.Id.checkVehicle);
                overBudget = row.FindViewById<TextView>(Resource.Id.vBudgetText);

                currentItem.TotalUpdated += (s, e) =>
                {
                    if (currentItem.Vehicle.IsOverBudget)
                    {
                        overBudget.Visibility = ViewStates.Visible;
                    }
                    else
                        overBudget.Visibility = ViewStates.Invisible;

                };

                checkBox.CheckedChange += (s, e) =>
                {
                    var cbSender = s as CheckBox;

                    if (cbSender != null && cbSender.Tag is VehicleWrapper && cbSender.Checked)
                    {
                        this.SelectedVehicle = (cbSender.Tag as VehicleWrapper).Vehicle;
                    }
                };

                if (currentItem.Vehicle.IsOverBudget)
                {
                    overBudget.Visibility = ViewStates.Visible;
                }
                else
                    overBudget.Visibility = ViewStates.Invisible;

            }
            else
                checkBox = row.FindViewById<CheckBox>(Resource.Id.checkVehicle);

            checkBox.Text = currentItem.Vehicle.Name;
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
            UpdateTables();
            SelectedVehicle = vm;
            NotifyDataSetChanged();
        }

        public void Remove(VehicleViewModel vm)
        {
            var index = Vehicles.IndexOf(vm) - 1;
            Vehicles.Remove(vm);
            UpdateTables();
            SelectedVehicle = Vehicles.ElementAtOrDefault(index);
            NotifyDataSetChanged();
        }

        public void Clear()
        {
            Vehicles.Clear();
            SelectedVehicle = null;
            CoreApp.Current.VehicleManager.Clear();
            NotifyDataSetChanged();
        }

        public void Sync()
        {
            CoreApp.Current.VehicleManager.SyncAsync();
            Vehicles = CoreApp.Current.VehicleManager.ToList();
            NotifyDataSetChanged();
        }

        private void UpdateTables()
        {
            CoreApp.Current.VehicleManager.Clear();

            if (Vehicles.Count > 0)
            foreach (var vm in Vehicles)
            {
                var obj = new VehicleViewModel(vm);
                CoreApp.Current.VehicleManager.Add(obj);
            }
        }

    }
}