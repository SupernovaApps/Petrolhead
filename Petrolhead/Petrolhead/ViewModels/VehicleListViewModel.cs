using GalaSoft.MvvmLight;
using Petrolhead.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petrolhead.ViewModels
{
    public sealed class VehicleListViewModel : ViewModelBase
    {
        public VehicleListViewModel()
        {
            Init();

            
        }

        private async void Init()
        {
#if !DEBUG
            Vehicles = new ObservableCollection<VehicleViewModel>((await DataStore.Current.GetVehiclesAsync()).Select<Models.Vehicle, VehicleViewModel>(x => x));
#else
            Vehicles = new ObservableCollection<VehicleViewModel>()
            {
                new VehicleViewModel(new Models.Vehicle() {
                    Name = "Dad's Car"
                }),
                new VehicleViewModel(new Models.Vehicle()
                {
                    Name = "Mum's Car",
                })
            };
#endif
            await Task.Delay(200);
            SelectedVehicle = Vehicles.ElementAtOrDefault(0);
        }

        private ObservableCollection<VehicleViewModel> _vehicles = new ObservableCollection<VehicleViewModel>();
        public ObservableCollection<VehicleViewModel> Vehicles
        {
            get
            {
                return _vehicles;
            }
            set
            {
                Set(ref _vehicles, value);
            }
        }

        private VehicleViewModel _selectedVehicle = null;

        public VehicleViewModel SelectedVehicle { get { return _selectedVehicle; } set { Set(ref _selectedVehicle, value); } }

        public async Task AddVehicle(VehicleViewModel v)
        {
            Vehicles.Add(v);

            await DataStore.Current.AddVehicleAsync(v);

                   
            SelectedVehicle = v;
            await Sync();
           
        }

        public async Task RemoveVehicle(VehicleViewModel v)
        {
            int index = Vehicles.IndexOf(v);
            Vehicles.Remove(v);

            await DataStore.Current.RemoveVehicleAsync(v);
            SelectedVehicle = Vehicles.ElementAtOrDefault(index);

            
            await Sync();

        }

        public async Task Sync()
        {
            await DataStore.Current.SyncVehiclesAsync();
            Vehicles = new ObservableCollection<VehicleViewModel>((await DataStore.Current.GetVehiclesAsync()).Select<Models.Vehicle, VehicleViewModel>(x => x));
        }
           
    }
}
