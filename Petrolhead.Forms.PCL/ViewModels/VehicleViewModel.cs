using GalaSoft.MvvmLight;
using Petrolhead.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petrolhead.ViewModels
{
    /// <summary>
    /// EventArgs class which should be used when the status of your vehicle is updated.
    /// </summary>
    public class VehicleStatusChangedEventArgs : EventArgs
    {
       
        public Vehicle Vehicle { get; private set; }

        public VehicleStatusChangedEventArgs(Vehicle vehicle)
        {
            Vehicle = vehicle;
        }
    }

    public sealed class VehicleViewModel : ViewModelBase
    {
        /// <summary>
        /// Creates a new VehicleViewModel.
        /// </summary>
        /// <param name="v">The vehicle to be created.</param>
        public VehicleViewModel(Vehicle v)
        {
            Vehicle = v;
            PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == "Vehicle")
                {
                    OnVehicleChanged();
                }
            };
        }

        private void OnVehicleChanged()
        {
            var handler = VehicleUpdated;
            if (handler != null)
                handler(this, new VehicleStatusChangedEventArgs(this));
        }

       

        public static implicit operator Vehicle(VehicleViewModel lhs)
        {
            return lhs.Vehicle;
        }

        public static implicit operator VehicleViewModel(Vehicle lhs)
        {
            var vm = new VehicleViewModel(lhs);
            App.Current.OnVehicleCreated(ref vm);
            return vm;
        }

        public event EventHandler<VehicleStatusChangedEventArgs> VehicleUpdated;


        private Vehicle _vehicle = default(Vehicle);

        /// <summary>
        /// A vehicle.
        /// </summary>
        public Vehicle Vehicle
        {
            get
            {
                return _vehicle;
            }
            set
            {
                Set(ref _vehicle, value);
            }
        }
    }
}
