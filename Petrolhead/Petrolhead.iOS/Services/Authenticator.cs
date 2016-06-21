using Microsoft.WindowsAzure.MobileServices;
using Petrolhead.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(Petrolhead.iOS.Services.Authenticator))]
namespace Petrolhead.iOS.Services
{
    public sealed class Authenticator : IAuthenticator
    {
        // Logged in user
        private MobileServiceUser user;
        public MobileServiceUser User { get { return user; } }

        private static Authenticator _currentAuthenticator = new Authenticator();
        public static Authenticator CurrentAuthenticator
        {
            get
            {
                return _currentAuthenticator;
            }
            private set
            {
                _currentAuthenticator = value;
            }
        }

        private Authenticator()
        {

        }

        public async Task<bool> LoginAsync()
        {
            var success = false;


            try
            {
                user = await DataStore.Current.CloudService.LoginAsync(UIApplication.SharedApplication.KeyWindow.RootViewController, MobileServiceAuthenticationProvider.MicrosoftAccount);
                if (user != null)
                {
                    UIAlertView avAlert = new UIAlertView("Sign-in complete!", "You are now logged in!", null, "OK", null);
                    avAlert.Show();
                }
                success = true;
            }
            catch (InvalidOperationException)
            {
                UIAlertView avAlert = new UIAlertView("Sign-in canceled...", "You canceled the sign-in process before it completed.", null, "OK", null);
                avAlert.Show();
            }
            catch (Exception ex)
            {
                UIAlertView avAlert = new UIAlertView("Authentication Error", "An error occurred during the authentication process. Error code: " + ex.Message, null, "OK", null);
                avAlert.Show();
            }
            return success;
        }

        public async Task<bool> LogoutAsync()
        {
            var success = false;
            try
            {
                await DataStore.Current.CloudService.LogoutAsync();
                user = null;
                success = true;
               
            }
            catch (Exception)
            {
                UIAlertView avAlert = new UIAlertView("Logout Failed", "You couldn't be logged out...", null, "OK", null);
            }
            return success;
        }
    }
}
