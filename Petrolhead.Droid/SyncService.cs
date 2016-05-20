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
using System.Threading;
using System.Threading.Tasks;
using Android.Util;

namespace Petrolhead
{
    [Service]
    public sealed class SyncService : IntentService
    {
       

        private async void StartSyncing()
        {
            try
            {
                Log.Debug("SyncService", "Syncing data...");
                if ((await DataStore.Current()).IsAuthenticated)
                    await CoreApp.Current.Vehicles.RefreshAsync();
            }
            catch (Exception ex)
            {
                Log.Debug("SyncService", "Sync failed - showing error message...");
                await CoreApp.Current.DialogHelper.ShowDialogAsync(ex);

            }

            new Task(() =>
            {
                try
                {
                    if (CoreApp.Current.Vehicles != null && CoreApp.Current.Vehicles.Count > 0)
                    {
                        NotificationManager nMgr = (NotificationManager)GetSystemService(NotificationService);
                        foreach (var v in CoreApp.Current.Vehicles)
                        {
                            if (v.Total > v.BudgetMax)
                            {
                                var notificationId = v.Id.GetHashCode() + ">BDGT".GetHashCode();
                                Notification.Builder builder = new Notification.Builder(ApplicationContext)
                                    .SetSmallIcon(Resource.Drawable.Icon)
                                    .SetTicker(v.Name + " is over budget!")
                                    .SetContentTitle("Budget Message")
                                    .SetContentText("Your " + v.Name + " is over budget! Better cut some costs!");
                                nMgr.Notify(notificationId, builder.Build());


                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }).Start();
        }

  
      
      

        protected override void OnHandleIntent(Intent intent)
        {
            new Task(() => { StartSyncing(); }).Start();
        }
    }
}