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

            Detail = new VehicleDetailsView(vm.SelectedVehicle);




            ToolbarItem addItem = new ToolbarItem();
            addItem.Text = "Add";
            addItem.Icon = Device.OnPlatform(default(string), "Resources/drawable/ic_add.png", "Assets/ic_add.png");
            addItem.Clicked += AddItem_Clicked;
            ToolbarItems.Add(addItem);



        }


        private async void AddItem_Clicked(object sender, EventArgs e)
        {
            var result = await DisplayActionSheet("This area is under active development!", "Cancel", null, new string[] { "Continue anyway" });

            if (result == "Continue anyway")
            {
                VehicleEditorPage page = new VehicleEditorPage();
                  await App.Current.MainPage.Navigation.PushModalAsync(page);

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
