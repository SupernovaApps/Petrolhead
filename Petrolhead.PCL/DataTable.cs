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

        public List<VehicleViewModel> GetVehicleList()
        {
            Initialize();
            return this.ToList();
        }

        public bool IsSyncing { get; private set; }
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

           
            for (int i = 0; i < CoreApp.Current.Vehicles.Count; i++)
            {
                VehicleViewModel vm = CoreApp.Current.Vehicles[i];
                OnVehicleCreated(ref vm);
                vm.UpdateVehicle();
                base.Add(vm);
            }

            if (Count > 0 && (SelectedVehicle == null || !Contains(SelectedVehicle)))
                SelectedVehicle = this[0];
                
            
        }

        public new async Task Add(VehicleViewModel item)
        {
            OnVehicleCreated(ref item);
            base.Add(item);
            item.UpdateVehicle();
            SelectedVehicle = item;
            await CoreApp.Current.Vehicles.CreateAsync(item);
        }

        public new async Task Remove(VehicleViewModel item)
        {
            var index = this.IndexOf(item) - 1;
            base.Remove(item);
            
            
            SelectedVehicle = this.ElementAtOrDefault(index);
             await CoreApp.Current.Vehicles.DeleteAsync(item); ;
        }

        public async Task Update(VehicleViewModel item)
        {
            var index = IndexOf(item);
            this[index] = item;
            item.UpdateVehicle();
            await CoreApp.Current.Vehicles.UpdateAsync(item);
        }

        public async Task ClearAll()
        {
           
                base.Clear();
                await CoreApp.Current.Vehicles.ClearAsync();
           
        }

        private async Task Sync() { await CoreApp.Current.Vehicles.RefreshAsync(); }
        public async Task SyncAsync()
        {
            IsSyncing = true;

            
            await Sync();



            try
            {
                base.Clear();
                for (int i = 0; i < CoreApp.Current.Vehicles.Count; i++)
                {
                    VehicleViewModel vm = CoreApp.Current.Vehicles[i];
                    OnVehicleCreated(ref vm);
                    vm.UpdateVehicle();
                    base.Add(vm);
                }
            }
            catch (Exception)
            {
                CoreApp.Current.DialogHelper.ShowDialog("An error occurred during sync - the local vehicle list could not be populated", "Whoops!");
            }
          
            IsSyncing = false;
           
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
            try
            {
                // Add the item to the observable collection
                Add(row);

                // Add the item to the sync table
                await InitializeAsync();
                await _controller.InsertAsync(row);
                
            }
            finally
            {

            }
        }

        public async Task ClearAsync()
        {
            base.Clear();
            await _controller.PurgeAsync(true);
            
        }

        /// <summary>
        /// Update a record in the async table
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public async Task UpdateAsync(T row)
        {
            try
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
           finally
            {

            }

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
            try
            {
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
                else
                {
                    
                }
            }
            catch (System.Net.Http.HttpRequestException)
            {
                CoreApp.Current.DialogHelper.ShowDialog("Petrolhead couldn't access the remote server.");
                
            }
        }
    }
}
