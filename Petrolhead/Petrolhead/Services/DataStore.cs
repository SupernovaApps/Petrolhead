using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Petrolhead.Helpers;
using Petrolhead.Models;
using Plugin.Connectivity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(Petrolhead.Services.DataStore))]
namespace Petrolhead.Services
{
    public class DataStore : IDataStore
    {

        public IAuthenticator Authenticator
        {
            get;
            set;

        }

        public bool IsAuthenticated
        {
            get
            {
                return CloudService.CurrentUser != null && !CloudService.IsTokenExpired();
            }
        }

        private static DataStore _current = new DataStore();
        public static DataStore Current
        {
            get
            {
                return _current;
            }
            private set
            {
                _current = value;
            }
        }
        public MobileServiceClient CloudService { get; set; }

        IMobileServiceSyncTable<Vehicle> vehicleTable;

        bool initialized = false;

        private DataStore()
        {
            
            CloudService = 
                new MobileServiceClient(Constant.ApplicationUri);
            Init();

        }

        public async Task Init()
        {
            initialized = true;
            var store = new MobileServiceSQLiteStore(Constant.DatabasePath);
            store.DefineTable<Vehicle>();
            store.DefineTable<Expense>();
            store.DefineTable<Refuel>();
            store.DefineTable<Repair>();
            store.DefineTable<Component>();

            await CloudService.SyncContext.InitializeAsync(store, new MobileServiceSyncHandler());
            vehicleTable = CloudService.GetSyncTable<Vehicle>();
        }

        public async Task<Vehicle> AddVehicleAsync(Vehicle vehicle)
        {
            if (!initialized)
                await Init();

            if (!IsAuthenticated)
                await Authenticator.LoginAsync();

            if (vehicle.Id == null)
                await vehicleTable.InsertAsync(vehicle);
            else
                vehicle = await UpdateVehicleAsync(vehicle);

            await SyncVehiclesAsync();
            await CloudService.SyncContext.PushAsync();
            return vehicle;
        }

        private async Task<Vehicle> UpdateVehicleAsync(Vehicle vehicle)
        {
            if (!initialized)
                await Init();
            if (!IsAuthenticated)
                await Authenticator.LoginAsync();
            await vehicleTable.UpdateAsync(vehicle);
            await SyncVehiclesAsync();
            await CloudService.SyncContext.PushAsync();
            return vehicle;
        }

        public async Task SyncVehiclesAsync()
        {
            try
            {
                if (!CrossConnectivity.Current.IsConnected || !Settings.NeedsSync)
                    return;
                if (!IsAuthenticated)
                    await Authenticator.LoginAsync();

                await vehicleTable.PullAsync("allVehicles", vehicleTable.CreateQuery());

                Settings.LastSync = DateTime.Now;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("SYNC FAILED : " + ex.Message);
            }
           
        }

        public async Task<bool> RemoveVehicleAsync(Vehicle vehicle)
        {
            if (!initialized)
                await Init();

            if (!IsAuthenticated)
                await Authenticator.LoginAsync();

            await vehicleTable.DeleteAsync(vehicle);
            await SyncVehiclesAsync();
            return true;
        }

        public async Task<IEnumerable<Vehicle>> GetVehiclesAsync()
        {
            if (!initialized)
                await Init();

            await vehicleTable.PullAsync("allVehicles", vehicleTable.CreateQuery());

            return await vehicleTable.ToEnumerableAsync();
        }
    }
}
