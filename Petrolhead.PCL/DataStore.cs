using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Petrolhead.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petrolhead
{
    public class DataStore
    {
        private DataStore()
        {

        }
        #region Static settings
        private static string CloudURI = "https://petrolhead.azurewebsites.net";
        private static string LocalStorePath = "localdata.db";
        #endregion

        #region Private instance variables
        private bool _isInitialized = false;
        #endregion

        public bool IsInitialized
        {
            get
            {
                return _isInitialized;
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                return CoreApp.Current.Authenticator.IsAuthenticated;
            }
        }

        public MobileServiceClient CloudService { get; private set; }

        public MobileServiceSQLiteStore LocalCache { get; private set; }

        public async Task<bool> AuthenticateAsync()
        {
            return await CoreApp.Current.Authenticator.AuthenticateAsync();
        }

        private async Task InitAsync()
        {
            CloudService = new MobileServiceClient(CloudURI);
            LocalCache = new MobileServiceSQLiteStore(LocalStorePath);
            LocalCache.DefineTable<Vehicle>();
            LocalCache.DefineTable<Expense>();
            LocalCache.DefineTable<Repair>();
            LocalCache.DefineTable<Refuel>();

            await CloudService.SyncContext.InitializeAsync(LocalCache);
        }

        private static DataStore _current = null;
        
        public static async Task<DataStore> Current()
        {
            if (_current == null)
            {
                _current = new DataStore();
                await _current.InitAsync();
            }
            return _current;
        }
    }
}
