using GalaSoft.MvvmLight.Messaging;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Petrolhead.Models;
using Petrolhead.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Petrolhead.Views
{
    public partial class VehicleEditorPage : ContentPage
    {
        public VehicleEditorPage()
        {
            InitializeComponent();

            BindingContext = new VehicleEditorViewModel();
            CreateButton.BackgroundColor = Color.Default;
            
        }

        public VehicleEditorPage(VehicleViewModel viewmodel)
        {
            InitializeComponent();
            BindingContext = new VehicleEditorViewModel(viewmodel);

            CreateButton.BackgroundColor = Color.Default;
        }

        public VehicleEditorViewModel ViewModel => this.BindingContext as VehicleEditorViewModel;
        public event EventHandler VehicleReady;

        private void OnVehicleReady()
        {
            EventHandler handler = VehicleReady;

            if (handler != null)
                handler(this, new EventArgs());
        }
        public async Task FinishAsync()
        {
            if (await ValidateAsync())
            {

                try
                {
                    await ((App.Current.MainPage as MasterDetailPage).Master.BindingContext as VehicleListViewModel).AddVehicle(ViewModel.Vehicle);
                }
                catch (MobileServicePushFailedException)
                {
                    await DisplayAlert("Sync Error", "Petrolhead encountered an error while syncing your vehicle data. Your data should still be available locally.", "OK");
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Vehicle Creation Error", "Petrolhead encountered an unidentified error while creating a new vehicle. Error code: " + ex.Message, "OK");
                    return;
                }
               

                await Navigation.PopModalAsync(true);
                
            }
        }

        private async Task<bool> ValidateAsync()
        {
            VehicleEditorViewModel vm = BindingContext as VehicleEditorViewModel;
            bool success = false;

            if (string.IsNullOrWhiteSpace(vm.Vehicle.Name))
            {
                await DisplayAlert("Invalid Data", "You can't create a vehicle without a name! How would you tell it apart?", "OK");
            }
            else
            {
                success = true;
            }
            return success;
        }
        private async void OnVehicleCreated(object sender, EventArgs args)
        {
            await FinishAsync();
        }
    }
}
