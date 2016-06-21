using GalaSoft.MvvmLight.Messaging;
using Petrolhead.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Petrolhead.Views
{
    public partial class VehicleDetailsView : ContentPage
    {
        public VehicleDetailsView(VehicleViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;

            if (vm != null && vm.Vehicle != null)
                Title = vm.Vehicle.Name ?? "Petrolhead";
            else
            {
                Title = "Petrolhead";
            }

           
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

                await App.Current.MainPage.Navigation.PushModalAsync(new VehicleEditorPage());
                
            }
        }
    }
}
