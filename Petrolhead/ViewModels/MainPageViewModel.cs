using Template10.Mvvm;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Navigation;
using NotificationsExtensions.Toasts;
using NotificationsExtensions;
using Windows.UI.Notifications;

namespace Petrolhead.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public MainPageViewModel()
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                Value = "Designtime value";
            }
        }

        string _Value = "Gas";
        public string Value { get { return _Value; } set { Set(ref _Value, value); } }


        private void OnVehicleCreated(ref VehicleViewModel vm)
        {
            vm.WarrantOverdue += OnWarrantOverdue;
            vm.TotalUpdated += OnTotalUpdated;
            vm.RegistrationOverdue += OnRegistrationOverdue;
        }

        private void OnRegistrationOverdue(object sender, VehicleUpdateEventArgs e)
        {
            // TODO: Implement this.
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            CoreApp.Initialize((App)App.Current, (App)App.Current, OnVehicleCreated);

            if (suspensionState.Any())
            {
                Value = suspensionState[nameof(Value)]?.ToString();
            }

            
            await Task.CompletedTask;
        }

        private void OnWarrantOverdue(object sender, VehicleUpdateEventArgs e)
        {
                var v = e.Vehicle;
                ToastContent content = new ToastContent()
                {
                    Visual = new ToastVisual()
                    {
                        BindingGeneric = new ToastBindingGeneric()
                        {
                            Children =
                            {
                                new AdaptiveText()
                                {
                                    HintStyle = AdaptiveTextStyle.HeaderSubtle,
                                    Text = "Warrant Overdue"
                                },
                                new AdaptiveText()
                                {
                                    HintStyle = AdaptiveTextStyle.Body,
                                    Text = v.Vehicle.Name + " is due for a warrant of fitness! Better get it down to the shop!"
                                }
                            }
                        }
                    },
                    Actions = new ToastActionsSnoozeAndDismiss(),
                    Scenario = ToastScenario.Alarm,
                };
            ToastNotification toast = new ToastNotification(content.GetXml());
            ToastNotificationManager.CreateToastNotifier().Show(toast);
            
        }

        private void OnTotalUpdated(object sender, VehicleUpdateEventArgs e)
        {
            var v = e.Vehicle;

            if (v.Vehicle.IsOverBudget)
            {
                ToastContent content = new ToastContent()
                {
                    Visual = new ToastVisual()
                    {
                        BindingGeneric = new ToastBindingGeneric()
                        {
                            Children =
                        {
                            new AdaptiveText()
                            {
                                HintStyle = AdaptiveTextStyle.Subheader,
                                Text = "Budget Warning",

                            },
                            new AdaptiveText()
                            {
                                HintStyle = AdaptiveTextStyle.Body,
                                HintWrap = true,
                                Text = v.Vehicle.Name + " is over budget! Better start cutting costs!",
                            }
                        }
                        }
                    },
                    Scenario = ToastScenario.Reminder,
                };
                ToastNotification toast = new ToastNotification(content.GetXml());
                ToastNotificationManager.CreateToastNotifier().Show(toast);
            }
           
        }

        public override async Task OnNavigatedFromAsync(IDictionary<string, object> suspensionState, bool suspending)
        {
            if (suspending)
            {
                suspensionState[nameof(Value)] = Value;
            }
            await Task.CompletedTask;
        }

        public override async Task OnNavigatingFromAsync(NavigatingEventArgs args)
        {
            args.Cancel = false;
            await Task.CompletedTask;
        }

        public void GotoDetailsPage() =>
            NavigationService.Navigate(typeof(Views.DetailPage), Value);

        public void GotoSettings() =>
            NavigationService.Navigate(typeof(Views.SettingsPage), 0);

        public void GotoPrivacy() =>
            NavigationService.Navigate(typeof(Views.SettingsPage), 1);

        public void GotoAbout() =>
            NavigationService.Navigate(typeof(Views.SettingsPage), 2);

    }
}

