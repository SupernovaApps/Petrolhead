using Petrolhead.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace Petrolhead.UWP.BackgroundTasks
{
    public sealed class SyncBackgroundTask : IBackgroundTask
    {
        DataTable<Vehicle> vehicles;
        BackgroundTaskDeferral deferral;
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            CoreApp.Initialize(null, new Authenticator());
            vehicles = new DataTable<Vehicle>();


            deferral = taskInstance.GetDeferral();
            

            await SyncAsync();

            

        }

        private async Task SyncAsync()
        {
            await vehicles.RefreshAsync();
            deferral.Complete();
        }
    }
}
