using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Petrolhead.Models;
using Petrolhead.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petrolhead.Data
{
    public class VehicleManager
    {

        private IMobileServiceSyncTable<Vehicle> _vehicleTable = null;

              
        private IAuthenticator _authenticator = null;
        protected IAuthenticator Authenticator
        {
            get
            {
                return _authenticator;
            }
            private set
            {
                _authenticator = value;
            }
        }

        

        private static VehicleManager _current = new VehicleManager();
        public static VehicleManager Current
        {
            get
            {
                return _current;
                
            }
            set
            {
                _current = value;
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                return (CurrentClient.CurrentUser != null && Authenticator.CurrentUser != null);
            }
        }

       

        private VehicleManager()
        {
            Current = this;
            ConfigureAsync();
            
        }

        protected async Task ConfigureAsync()
        {
            CurrentClient = new MobileServiceClient(Constants.ApplicationURI);
            MobileServiceSQLiteStore store = new MobileServiceSQLiteStore(Constants.DBPath);
            store.DefineTable<Vehicle>();
            store.DefineTable<Expense>();
            store.DefineTable<Reminder>();
            await CurrentClient.SyncContext.InitializeAsync(store);
            _vehicleTable = CurrentClient.GetSyncTable<Vehicle>();
                        
        }

        public async Task CreateVehicleAsync(VehicleViewModel v)
        {
            await _vehicleTable.InsertAsync(v);
        }

        public async Task DeleteVehicleAsync(VehicleViewModel v)
        {
            await _vehicleTable.DeleteAsync(v);
        }

        public async Task UpdateVehicleAsync(VehicleViewModel v)
        {
            await _vehicleTable.UpdateAsync(v);
        }

        public async Task<ObservableCollection<VehicleViewModel>> GetVehiclesAsync()
        {
            await SyncAsync();

            ObservableCollection<VehicleViewModel> vehicles = new ObservableCollection<VehicleViewModel>();
            try
            {
                vehicles = new ObservableCollection<VehicleViewModel>((await _vehicleTable.ToEnumerableAsync()).Select<Vehicle, VehicleViewModel>(x => new VehicleViewModel(x)));
            }
            catch (NullReferenceException)
            {

            }
            catch (ArgumentException)
            {

            }
            catch (Exception ex)
            {
                throw new PetrolheadException(ex);
            }
           
            return vehicles;
        }

        public async Task<bool> SyncAsync()
        {
            bool success = false;

            try
            {
                await _vehicleTable.PullAsync("vehicles", _vehicleTable.CreateQuery());
            }
            catch (MobileServicePushFailedException)
            {

            }

            try
            {
                await CurrentClient.SyncContext.PushAsync();
                success = true;
            }
            catch
            {

            }
            return success;
        }
        public async Task<bool> AuthenticateAsync()
        {

            return await Authenticator.AuthenticateAsync();

        }

        

        private MobileServiceClient _client = null;
        public MobileServiceClient CurrentClient
        {
            get
            {
                return _client;
            }
            set
            {
                _client = value;
            }
        }

        public static void Initialize(IAuthenticator authenticator)
        {
            Current.Authenticator = authenticator;
            
        }




    }
}
