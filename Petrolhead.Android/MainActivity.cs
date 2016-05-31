using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;
using Android.Support.V4.App;
using Petrolhead.ViewModels;

namespace Petrolhead.Android
{
    [Activity(Label = "Petrolhead.Android", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity, IAuthenticator, IDialogHelper
    {
        int count = 1;

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
                        NotificationCompat.Builder builder = new NotificationCompat.Builder(this)
                            .SetSmallIcon(Resource.Drawable.Icon)
                            .SetContentTitle("Sign-in Failure")
                            .SetContentText("Petrolhead couldn't sign you in.")
                            .SetTicker("Whoops!");
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

        public void ShowDialogAsync(string content)
        {
            RunOnUiThread(() =>
            {
                new AlertDialog.Builder(this)
                .SetMessage(content)
                .Show();
            });
        }

        public void ShowDialogAsync(string content, string title)
        {
            RunOnUiThread(() =>
            {
                new AlertDialog.Builder(this)
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

            if (e.Vehicle != null)
            {
                if (v.Vehicle.IsOverBudget)
                {
                    NotificationManager notificationMgr = (NotificationManager)GetSystemService(NotificationService);

                    int notificationId = (v.Vehicle.Name + "OVERBDGT").GetHashCode();
                    NotificationCompat.Builder builder = new NotificationCompat.Builder(this)
                        .SetContentTitle("Budget Warning")
                        .SetContentText(v.Vehicle.Name + " is over budget!")
                        .SetTicker(v.Vehicle.Name + " is over budget!")
                        .SetSmallIcon(Resource.Drawable.Icon);
                    Notification notification = builder.Build();
                    notificationMgr.Notify(notificationId, notification);

                }
            }
        }

        private void OnRegistrationOverdue(object sender, VehicleUpdateEventArgs e)
        {
            
        }

        private void OnWarrantOverdue(object sender, VehicleUpdateEventArgs e)
        {
            
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            CurrentPlatform.Init();

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            CoreApp.Initialize(this, this, OnVehicleUpdated);
            
            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.MyButton);

            button.Click += async delegate { await CoreApp.Current.VehicleValidator.ValidateAsync(new Models.Vehicle()); };
            CoreApp.Current.LoginAsync();
        }

        protected override void OnStart()
        {
            base.OnStart();
          

        }
    }
}

