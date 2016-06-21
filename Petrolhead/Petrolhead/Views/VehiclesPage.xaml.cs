using GalaSoft.MvvmLight.Messaging;
using Petrolhead.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIKit;
using Xamarin.Forms;

namespace Petrolhead.Views
{
    public partial class VehiclesPage : MasterDetailPage
    {
        public VehiclesPage()
        {
            InitializeComponent();

            Messenger.Default.Register<NotificationMessage<VehicleViewModel>>(this, (message) =>
            {
                switch (message.Notification)
                {
                    case "Update-Details":
                        {
                            IsPresented = false;
                            break;
                        }
                }
            });

            var vm = new VehicleListViewModel();
            var masterPage = new VehicleListView(vm);
            _listModel = vm;
            masterPage.ListView.ItemSelected += OnVehicleSelected;

            Master = masterPage;

            if (Device.OS != TargetPlatform.Android)
                Detail = new NavigationPage(new VehicleDetailsView(vm.SelectedVehicle));
            else
                Detail = new VehicleDetailsView(vm.SelectedVehicle);

          




        }

        private async void AddItem_Clicked(object sender, EventArgs e)
        {
            var result = await DisplayActionSheet("This area is under active development!", "Cancel", null, new string[] { "Continue anyway" });

            if (result == "Continue anyway")
            {

                await App.Current.MainPage.Navigation.PushModalAsync(new VehicleEditorPage());

            }
        }

        private VehicleListViewModel _listModel = null;
        public VehicleListViewModel ListModel
        {
            get
            {
                return _listModel;
            }
        }

        private async void OnVehicleSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var vehicle = e.SelectedItem as VehicleViewModel;

            if (vehicle != null)
            {
                Detail = new NavigationPage(new VehicleDetailsView(vehicle));
            }
            else
            {
                await DisplayAlert("Invalid Vehicle", "You selected an invalid vehicle.", "OK");
            }

        }
    }
}
