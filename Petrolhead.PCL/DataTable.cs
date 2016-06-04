using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Petrolhead.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petrolhead
{
    public delegate void OnVehicleCreated(ref VehicleViewModel vm);

    public sealed class VehicleManager : ObservableCollection<VehicleViewModel>, INotifyPropertyChanged
    {
        public VehicleManager(OnVehicleCreated onCreate)
        {
            this.OnVehicleCreated = onCreate;
            Initialize();
        }

        public new event PropertyChangedEventHandler PropertyChanged;

        public OnVehicleCreated OnVehicleCreated { get; set; }

        private VehicleViewModel selectedVehicle = null;
        public VehicleViewModel SelectedVehicle
        {
            get
            {
                return selectedVehicle;

            }
            set
            {
                selectedVehicle = value;
                OnPropertyChanged(nameof(SelectedVehicle));
            }
        }

        private void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        private async void Initialize()
        {
            base.Clear();

            await CoreApp.Current.Vehicles.RefreshAsync();

            
            if (OnVehicleCreated == null)
                throw new NullReferenceException("Must have OnVehicleCreated delegate!");

            if (CoreApp.Current.Vehicles.Count < 1)
                return;

            for (int i = 0; i < CoreApp.Current.Vehicles.Count; i++)
            {
                VehicleViewModel vm = CoreApp.Current.Vehicles[i];
                OnVehicleCreated(ref vm);
                base.Add(vm);
            }

            if (SelectedVehicle == null || !Contains(SelectedVehicle))
                SelectedVehicle = this[0];
                
            
        }

        public new async void Add(VehicleViewModel item)
        {
            OnVehicleCreated(ref item);
            base.Add(item);
            await CoreApp.Current.Vehicles.CreateAsync(item);
            SelectedVehicle = item;
        }

        public new async void Remove(VehicleViewModel item)
        {
            var index = this.IndexOf(item) - 1;
            base.Remove(item);
            await CoreApp.Current.Vehicles.DeleteAsync(item);
            
            SelectedVehicle = this.ElementAtOrDefault(index);
            
        }

        public async void Update(VehicleViewModel item)
        {
            var index = IndexOf(item);
            this[index] = item;
            await CoreApp.Current.Vehicles.UpdateAsync(item);
        }

        public async void SyncAsync()
        {
            Task t = new Task(async () => await CoreApp.Current.Vehicles.RefreshAsync());
            t.Start();
            while (!t.IsCompleted)
                await Task.Delay(1000);
            Initialize();
        }
    }

    public sealed class DataTable<T> : ObservableCollection<T>
    {
        private static IMobileServiceSyncTable<T> _controller = null;

       
        
        private async Task InitializeAsync()
        {
            if (_controller == null)
            {
                var store = await DataStore.Current();
                _controller = store.CloudService.GetSyncTable<T>();

                // Read the stuff in the table already
                var list = await _controller.ToListAsync();
                var iterator = list.GetEnumerator();
                while (iterator.MoveNext())
                {
                    Add(iterator.Current);
                }
            }
        }

        /// <summary>
        /// Add a record to the async table
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public async Task CreateAsync(T row)
        {
            // Add the item to the observable collection
            Add(row);

            // Add the item to the sync table
            await InitializeAsync();
            await _controller.InsertAsync(row);
        }

        public async Task ClearAsync()
        {
            Clear();
            await _controller.PurgeAsync(true);

        }

        /// <summary>
        /// Update a record in the async table
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public async Task UpdateAsync(T row)
        {
            // Update the record in the observable collection
            for (var idx = 0; idx < Count; idx++)
            {
                if (Items[idx].Equals(row))
                {
                    Items[idx] = row;
                }
            }

            // Update the record in the sync table
            await InitializeAsync();
            await _controller.UpdateAsync(row);

        }

        /// <summary>
        /// Delete a record in the sync table
        /// </summary>
        /// <param name="row">Item being removed</param>
        /// <returns>
        /// Task (async)
        /// </returns>
        public async Task DeleteAsync(T row)
        {
            // Remove this item from the observable collection
            Remove(row);

            // Remove this item from the sync table
            await InitializeAsync();
            await _controller.DeleteAsync(row);

        }

        /// <summary>
        /// Refresh the async table from the cloud
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public async Task RefreshAsync()
        {
            await InitializeAsync();

            var store = await DataStore.Current();
            if (store.IsAuthenticated)
            {
                try
                {
                    // Do the Pushes
                    await store.CloudService.SyncContext.PushAsync();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(string.Format("EXCEPTION:{0}", ex.Message));
                }

                // Do the pulls
                await _controller.PullAsync("tablequery", _controller.CreateQuery());
            }
        }
    }
}
