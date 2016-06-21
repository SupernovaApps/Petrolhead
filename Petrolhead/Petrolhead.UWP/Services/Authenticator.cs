using Microsoft.WindowsAzure.MobileServices;
using Petrolhead.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Credentials;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Xamarin.Forms;

[assembly: Dependency(typeof(Petrolhead.UWP.Services.Authenticator))]
namespace Petrolhead.UWP.Services
{
    public sealed class Authenticator : IAuthenticator
    {
        MobileServiceUser user;

        private bool LoginWithStoredCredentialsAsync()
        {
            

            PasswordVault vault = new PasswordVault();
            PasswordCredential credential = null;

            try
            {
                credential = vault.FindAllByResource(MobileServiceAuthenticationProvider.MicrosoftAccount.ToString()).FirstOrDefault();
            }
            catch (Exception)
            {
                // Ignore errors in this part of the code.
            }

            if (credential != null)
            {
                user = new MobileServiceUser(credential.UserName);
                credential.RetrievePassword();
                user.MobileServiceAuthenticationToken = credential.Password;
                DataStore.Current.CloudService.CurrentUser = user;

                if (DataStore.Current.CloudService.IsTokenExpired())
                    return false;
                return true;
            }

            return false;

        }

        private async Task<bool> LoginWithNewCredentials()
        {
            try
            {
                user = await DataStore.Current.CloudService.LoginAsync(MobileServiceAuthenticationProvider.MicrosoftAccount);

                PasswordVault vault = new PasswordVault();
                PasswordCredential credential = null;
                if (user != null)
                {

                    await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                    {
                        await new MessageDialog("You are now logged in to Petrolhead!", "Booyah!").ShowAsync();

                    });

                    credential = new PasswordCredential(MobileServiceAuthenticationProvider.MicrosoftAccount.ToString(), user.UserId, user.MobileServiceAuthenticationToken);
                    vault.Add(credential);
                    return true;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return false;
        }

        public async Task<bool> LoginAsync()
        {


            try
            {
                if (LoginWithStoredCredentialsAsync())
                    return true;
                else
                {
                    if (await LoginWithNewCredentials())
                        return true;
                    return false;
                }

                    

            }
            catch (InvalidOperationException)
            {
                await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    await new MessageDialog("You canceled the authentication process before it could complete.", "Login Canceled").ShowAsync();
                });
            }
            catch (Exception ex)
            {
                await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    await new MessageDialog("The authentication process ended with an error. Error code: " + ex.Message, "Login Failed").ShowAsync();
                });
            }
            return false;
        }



        public async Task<bool> LogoutAsync()
        {
            try
            {
                await DataStore.Current.CloudService.LogoutAsync();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        private static Authenticator _current = null;
        public static Authenticator CurrentAuthenticator
        {
            get
            {
                if (_current == null)
                    _current = new Authenticator();
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
    }
}
