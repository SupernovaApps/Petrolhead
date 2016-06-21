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
    public partial class VehicleListView : ContentPage
    {
        public ListView ListView { get { return vehicleList; } }

        public VehicleListView(VehicleListViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

       
    }
}
