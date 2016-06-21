using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Petrolhead.Services;
using Xamarin.Forms;
using Petrolhead.Droid.Services;
using Microsoft.WindowsAzure.MobileServices;

[assembly: Dependency(typeof(Authenticator))]
namespace Petrolhead.Droid.Services
{

    public sealed class Authenticator : IAuthenticator

    {
        private static Authenticator _current = new Authenticator();
        public static Authenticator Current
        {
            get
            {
                return _current;
            }
            private set
            {
                _current = value;
            }
        }

        private Authenticator()
        {

        }

        private MobileServiceUser user;
        public async Task<bool> LoginAsync()
        {
            bool success = false;

            try
            {
                user = await DataStore.Current.CloudService.LoginAsync(Forms.Context, MobileServiceAuthenticationProvider.MicrosoftAccount);

                if (user != null)
                {
                    AlertDialog.Builder builder = new AlertDialog.Builder(Forms.Context)
                        .SetTitle("Signed in!")
                        .SetMessage("You are now signed in!");
                    builder.Show();
                    success = true;
                }
              
            }
            catch (InvalidOperationException)
            {
                new AlertDialog.Builder(Forms.Context)
                    .SetTitle("Authentication Canceled")
                    .SetMessage("You canceled the sign-in process before it could complete.")
                    .Show();
            }
            catch (Exception ex )
            {
                new AlertDialog.Builder(Forms.Context)
                    .SetTitle("Authentication Error")
                    .SetMessage("An error occurred during the authentication process. Error message: " + ex.Message)
                    .Show();
            }
            return success;
        }

        public async Task<bool> LogoutAsync()
        {
            await DataStore.Current.CloudService.LogoutAsync();
            return true;
        }
    }
}