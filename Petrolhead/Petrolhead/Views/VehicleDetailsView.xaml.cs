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

           
            

          
            
        }

      
    }
}
