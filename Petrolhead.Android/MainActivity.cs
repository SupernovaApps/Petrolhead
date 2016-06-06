﻿using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;
using Petrolhead.ViewModels;
using Android.Locations;
using System.Collections.Generic;
using System.Linq;
using Android.Support.V7.App;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Messenger = GalaSoft.MvvmLight.Messaging.Messenger;
using AlertDialog = Android.Support.V7.App.AlertDialog;
using System.Threading;
using GalaSoft.MvvmLight.Messaging;
using Android.Support.V7.Widget;
using static Android.Support.V7.Widget.Toolbar;

namespace Petrolhead
{
    [Activity(Label = "Petrolhead", MainLauncher = true, Icon = "@drawable/icon", Theme = "@style/AppTheme"
        )]
    public class MainActivity : AppCompatActivity, IAuthenticator, IDialogHelper
    {
        

        private VehicleAdapter adapter;

        public bool IsAuthenticated
        {
            get
            {
                return (User != null);
            }
        }

        public MobileServiceUser User
        {
            get; set;
        }

        const int SignInFailedNotificationId = 1342;
      

        public async Task<bool> AuthenticateAsync()
        {
            bool success = false;

            DataStore store = await DataStore.Current();
            try
            {
                RunOnUiThread(async () =>
                {
                    try
                    {
                        User = await store.CloudService.LoginAsync(this, MobileServiceAuthenticationProvider.MicrosoftAccount);
                    }
                    catch (InvalidOperationException)
                    {
                        Toast.MakeText(this, "Login was canceled by the user.", ToastLength.Long);
                    }
                    catch (Exception)
                    {
                        NotificationManager notificationMgr = (NotificationManager)GetSystemService(Context.NotificationService);
                        NotificationCompat.Builder builder = new NotificationCompat.Builder(this);
                        builder.SetContentTitle("Login Failure")
                        .SetContentText("Petrolhead couldn't log you in...")
                        .SetSmallIcon(Resource.Drawable.Icon)
                        .SetContentIntent(PendingIntent.GetActivity(this, 0, new Intent(this, typeof(MainActivity)), 0));
                        Notification notification = builder.Build();
                        notificationMgr.Notify(SignInFailedNotificationId, notification);
                    }
                });
                success = true;
            }
            catch
            {
               
            }

            return success;
        }

        public void ShowDialog(string content)
        {
            RunOnUiThread(() =>
            {
                new Android.Support.V7.App.AlertDialog.Builder(this)
                .SetMessage(content)
                .Show();
            });
        }

        public void ShowDialog(string content, string title)
        {
            RunOnUiThread(() =>
            {
                new Android.Support.V7.App.AlertDialog.Builder(this)
                .SetMessage(content)
                .SetTitle(title)
                .Show();
            });
        }

        public void OnVehicleUpdated(ref VehicleViewModel vm)
        {
            vm.WarrantOverdue += OnWarrantOverdue;
            vm.RegistrationOverdue += OnRegistrationOverdue;
            vm.TotalUpdated += OnTotalUpdated;
        }

        private void OnTotalUpdated(object sender, VehicleUpdateEventArgs e)
        {
            var v = e.Vehicle;

            if (v != null)
            {
                if (v.Vehicle.IsOverBudget)
                {
                    NotificationManager notificationMgr = (NotificationManager)GetSystemService(NotificationService);

                    int notificationId = (v.Vehicle.Name + "OVERBDGT").GetHashCode();
                    NotificationCompat.Builder builder = new NotificationCompat.Builder(this);
                        builder.SetContentTitle("Budget Warning")
                        .SetContentText(v.Vehicle.Name + " is over budget!")
                        .SetTicker(v.Vehicle.Name + " needs attention!")
                        .SetSmallIcon(Resource.Drawable.Icon)
                        .SetContentIntent(PendingIntent.GetActivity(this, 0, new Intent(this, typeof(MainActivity)), 0));
                    Notification notification = builder.Build();
                    notificationMgr.Notify(notificationId, notification);

                }
            }
        }

        private void OnRegistrationOverdue(object sender, VehicleUpdateEventArgs e)
        {
            var v = e.Vehicle;

            if (v != null)
            {
                NotificationManager notificationMgr = (NotificationManager)GetSystemService(NotificationService);

                int notificationId = (v.Vehicle.Name + "REGODUE").GetHashCode();
                NotificationCompat.Builder builder = new NotificationCompat.Builder(this);
                    builder.SetContentTitle("Registration Overdue")
                    .SetContentText(v.Vehicle.Name + " needs it's registration renewed!")
                    .SetSmallIcon(Resource.Drawable.Icon)
                    .SetTicker(v.Vehicle.Name + " needs attention!")
                    .SetContentIntent(PendingIntent.GetActivity(this, 0, new Intent(this, typeof(MainActivity)), 0));
                Notification notification = builder.Build();
                notificationMgr.Notify(notificationId, notification);
              
            }
        }


      
        private void OnWarrantOverdue(object sender, VehicleUpdateEventArgs e)
        {
            var v = e.Vehicle;

            if (v != null)
            {
                NotificationManager notificationMgr = (NotificationManager)GetSystemService(NotificationService);

                int notificationId = (v.Vehicle.Name + "WARRANTDUE").GetHashCode();
                NotificationCompat.Builder builder = new NotificationCompat.Builder(this);
                    builder.SetContentTitle("Warrant Reminder")
                    .SetContentText(v.Vehicle.Name + " needs a warrant!")
                    .SetSmallIcon(Resource.Drawable.Icon)
                    .SetTicker(v.Vehicle.Name + " needs attention!")
                    .SetContentIntent(PendingIntent.GetActivity(this, 0, new Intent(this, typeof(MainActivity)), 0));
                Notification notification = builder.Build();
                notificationMgr.Notify(notificationId, notification);

            }
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);


            CurrentPlatform.Init();

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);



            CoreApp.Initialize(this, this, OnVehicleUpdated);

            Toolbar toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);


            SetSupportActionBar(toolbar);


            var toolbarBottom = FindViewById<Toolbar>(Resource.Id.toolbar_bottom);
            toolbarBottom.Title = "Vehicle Options";
            toolbarBottom.InflateMenu(Resource.Menu.edit);
           


            
            toolbarBottom.MenuItemClick += (s, e) =>
            {
               switch (e.Item.ItemId)
                {
                    case Resource.Id.menu_edit:
                        {
                            if (!IsSelected())
                                return;
                            else
                            {
                                Intent intent = new Intent(this, typeof(VehicleInfoPage));
                                StartActivity(intent);
                            }
                            break;
                        }
                    case Resource.Id.menu_delete:
                        {
                            if (!IsSelected())
                                return;
                            else
                                Delete();
                            break;
                        }
                    case Resource.Id.menu_clear:
                        Clear();
                        break;
                }
                
                
            };




            adapter = new VehicleAdapter(this, Resource.Layout.VehicleRow);
            var listView = FindViewById<ListView>(Resource.Id.vehicleList);
            

            listView.Adapter = adapter;

            Messenger.Default.Register<NotificationMessage<VehicleViewModel>>(this, (message) =>
            {
                switch (message.Notification)
                {
                    case "Add-Vehicle":
                        {
                            AddItem(new VehicleWrapper(message.Content));
                            break;
                        }
                }
            });



            OnRefreshRequested();



        }

        public async void OnRefreshRequested()
        {
            await SyncAsync();
                       
            await RefreshDataFromTable();
        }

        private bool IsSelected()
        {
            if (adapter.SelectedVehicle == null)
            {
                AlertDialog.Builder builder = new AlertDialog.Builder(this)
                    .SetMessage("You need to select a vehicle to continue. Please select a vehicle and try again.")
                    .SetTitle("No Vehicle Selected");
                builder.Show();
                return false;
                    
            }
            return true;
        }

        public void Delete()
        {
            Android.Support.V7.App.AlertDialog.Builder builder = new Android.Support.V7.App.AlertDialog.Builder(this)
                   .SetTitle("Delete " + adapter.SelectedVehicle.Vehicle.Name + "?")
                   .SetMessage("Once a vehicle has been deleted, there is no way to recover it. Are you quite sure you want to delete this vehicle?")
                   .SetPositiveButton("I'm sure!", (sender, args) =>
                   {
                       RemoveItem(new VehicleWrapper(adapter.SelectedVehicle));
                   })
                   .SetNegativeButton("Don't do it!", (sender, args) =>
                   {
                        // no implementation required.
                    });
            builder.Show();
        }

        private async Task RefreshDataFromTable()
        {
            try
            {
                var vehicles = CoreApp.Current.VehicleManager.ToList();

                adapter.Clear();

                foreach (var vehicle in vehicles)
                    adapter.Add(vehicle);
                await Task.Delay(100);
            }
            catch (Exception)
            {
                ShowDialog("An error occurred while Petrolhead was populating the vehicle list.", "Data Access Error");
            }

        }

        public void Clear()
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this)
                .SetTitle("Are you sure?")
                .SetMessage("If you continue, Petrolhead will erase all your vehicle data. You won't be able to recover it! Do you still want to continue?")
                .SetPositiveButton("Yes", async (s, e) =>
                {
                    try
                    {
                        adapter.Clear();
                        await CoreApp.Current.VehicleManager.ClearAll();
                    }
                    catch
                    {

                    }
                })
                .SetNegativeButton("No", (s, e) => { });
            builder.Show();
        }


        public async Task SyncAsync()
        {
            await CoreApp.Current.VehicleManager.SyncAsync();
        }

        
        public async void AddItem(VehicleWrapper vm)
        {
           
                try
                {
                    adapter.Add(vm.Vehicle);
                    await CoreApp.Current.VehicleManager.Add(vm.Vehicle);
                    await SyncAsync();
                    
                }
                catch (Exception ex)
                {
                    ShowDialog("Vehicle could not be added. Error code: " + ex.Message, "Whoops!");
                }
            
        }

       
        public async void RemoveItem(VehicleWrapper vm)
        {
            try
            {
                await CoreApp.Current.VehicleManager.Remove(vm.Vehicle);
                await SyncAsync();
                adapter.Remove(vm.Vehicle);
            }
            catch
            {

            }
        }
        protected override void OnStop()
        {
            base.OnStop();
            
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.home, menu);
            var addMenuItem = menu.FindItem(Resource.Id.menu_add);
            

            return base.OnCreateOptionsMenu(menu);
        }



        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.menu_add)
            {
               
                Intent intent = new Intent(this, typeof(VehicleCreatorActivity));
                StartActivity(intent);
            }
            return base.OnOptionsItemSelected(item);
        }

        


    }
}

