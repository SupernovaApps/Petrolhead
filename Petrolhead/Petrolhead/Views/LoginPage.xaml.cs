using Petrolhead.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Petrolhead.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            BindingContext = new ViewModels.LoginPageViewModel();
            loginButton.Clicked += (s, e) => (BindingContext as ViewModels.LoginPageViewModel).Authenticate();
        }

      
    }
}
