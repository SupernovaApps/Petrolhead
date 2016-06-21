using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Petrolhead.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Petrolhead.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        public LoginPageViewModel()
        {

        }

        private RelayCommand _authCommand = default(RelayCommand);
        public RelayCommand AuthCommand { get { return _authCommand ?? (_authCommand = new RelayCommand(Authenticate)); } }

        public async void Authenticate()
        {
            var success = await DataStore.Current.Authenticator.LoginAsync();

            if (success)
            {

                App.Current.MainPage = new NavigationPage(new Views.VehiclesPage());
                
            }
        }
    }
}
