using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;
using Android.Accounts;
using Petrolhead.Models;
using System.Collections.Generic;
using Java.Lang;
using System.Timers;

namespace Petrolhead
{
    
    [Activity(Label = "Petrolhead", MainLauncher = true, Icon = "@drawable/icon")]
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

        public async Task<bool> AuthenticateAsync()
        {
            bool success = false;

            try
            {
                var store = await DataStore.Current();
                
                    User = await store.CloudService.LoginAsync(this, MobileServiceAuthenticationProvider.MicrosoftAccount);
                    Toast.MakeText(this, "You are now logged in!", ToastLength.Long);
                success = true;
            }
            catch (InvalidOperationException invalidOp)
            {
                Toast.MakeText(this, "Sign-in was canceled by the user.", ToastLength.Long);
            }
            catch (System.Exception ex)
            {
                await CoreApp.Current.DialogHelper.ShowDialogAsync("An unknown error occurred during authentication: " + ex.Message, "Whoops!");
            }
            return success;
        }

        public async Task<bool> ShowDialogAsync(System.Exception ex)
        {
            RunOnUiThread(() =>
            {
                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                builder.SetTitle(Resource.String.Whoops);
                builder.SetMessage(ex.Message);
                builder.Show();
            });
           
            return (await Task.FromResult(true));
        }

        public async Task<bool> ShowDialogAsync(string content)
        {
            RunOnUiThread(() =>
            {
                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                builder.SetTitle(Resource.String.Whoops);
                builder.SetMessage(content);
                builder.Show();
            });
          
            return (await Task.FromResult(true));
        }

        public async Task<bool> ShowDialogAsync(string content, string title)
        {
            RunOnUiThread(() =>
            {
                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                builder.SetTitle(title);
                builder.SetMessage(content);
                builder.Show();
            });
           
            return (await Task.FromResult(true));
        }

        

      

        public async Task<bool> ShowDialogAsync(string content, IList<DialogButton> actions)
        {
            RunOnUiThread(() =>
            {
                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                builder.SetMessage(content);

                if (actions != null)
                {
                    if (actions.Count > 3)
                        throw new ArgumentOutOfRangeException();


                    builder.SetPositiveButton(actions[0].Label, (s, e) =>
                    {
                        actions[0].Action?.Invoke();
                    });

                    builder.SetNeutralButton(actions[1].Label, (s, e) =>
                    {
                        actions[1].Action?.Invoke();
                    });

                    builder.SetNegativeButton(actions[2].Label, (s, e) =>
                    {
                        actions[2].Action?.Invoke();
                    });

                }
                builder.Show();
            });
          
            return (await Task.FromResult(true));
        }

        public async Task<bool> ShowDialogAsync(string content, string title, IList<DialogButton> actions)
        {
            RunOnUiThread(() =>
            {
                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                builder.SetMessage(content);
                builder.SetTitle(title);
                if (actions != null)
                {
                    if (actions.Count > 3)
                        throw new ArgumentOutOfRangeException();


                    builder.SetPositiveButton(actions[0].Label, (s, e) =>
                    {
                        actions[0].Action?.Invoke();
                    });

                    builder.SetNeutralButton(actions[1].Label, (s, e) =>
                    {
                        actions[1].Action?.Invoke();
                    });

                    builder.SetNegativeButton(actions[2].Label, (s, e) =>
                    {
                        actions[2].Action?.Invoke();
                    });

                }
                builder.Show();
            });
          
            return (await Task.FromResult(true));
        }



        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            CurrentPlatform.Init();



            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // initialize the PCL
            CoreApp.Initialize((IDialogHelper)this, (IAuthenticator)this);

            Vehicle v = new Vehicle();
            v.Name = "Ford Mondeo";
            v.BudgetMax = 1;
            v.Expenses.Add(new Expense() { Name = "test", Cost = 2 });



           



            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.MyButton);

            
            button.Click += async delegate { await CoreApp.Current.ExpenseValidator.ValidateAsync(new Models.Expense()); };

            syncSvcIntent = new Intent(this, typeof(SyncService));

            if (!IsAlarmSet())
            {
                var alarm = (AlarmManager)GetSystemService(AlarmService);
                var pendingSvcIntent = PendingIntent.GetService(Application.Context, 0, syncSvcIntent, PendingIntentFlags.CancelCurrent);
                alarm.SetRepeating(AlarmType.Rtc, 0, AlarmManager.IntervalHalfHour, pendingSvcIntent);

            }
            
        }

        Intent syncSvcIntent;
        private bool IsAlarmSet()
        {
            return PendingIntent.GetBroadcast(this, 0, syncSvcIntent, PendingIntentFlags.NoCreate) != null;
        }
    }
    
}

