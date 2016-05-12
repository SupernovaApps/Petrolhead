using Microsoft.HockeyApp;
using Microsoft.WindowsAzure.MobileServices;
using Petrolhead.Data;
using Petrolhead.Models;
using Petrolhead.UWP.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Credentials;
using Windows.UI.Popups;

namespace Petrolhead.UWP
{
    public sealed class Authenticator : Data.IAuthenticator
    {

        private User _currentUser = new User();
        public User CurrentUser { get { return _currentUser; } set { _currentUser = value; } }

      

        public async Task<bool> AuthenticateAsync()
        {
            bool success = false;
            try
            {
                if (!ConnectionHelper.Current.IsConnected)
                {
                    await App.Current.NavigationService.Dispatcher.DispatchAsync(async () =>
                    {
                        await new MessageDialog("I'm terribly sorry, but you couldn't be signed in. You aren't connected to the Internet at the moment. Please check your Internet connection and try again.", "Whoops!").ShowAsync();
                    });
                    return false;
                }

                success = true;
                PasswordVault vault = new PasswordVault();
                PasswordCredential credential = null;

                try
                {
                    credential = vault.FindAllByResource(MobileServiceAuthenticationProvider.MicrosoftAccount.ToString()).FirstOrDefault();
                }
                catch (COMException)
                {

                }
                catch (Exception)
                {

                }

                if (credential == null)
                {
                    try
                    {
                        CurrentUser.ServiceUser = await VehicleManager.Current.CurrentClient.LoginAsync(MobileServiceAuthenticationProvider.MicrosoftAccount.ToString());
                        credential = new PasswordCredential(MobileServiceAuthenticationProvider.MicrosoftAccount.ToString(), CurrentUser.ServiceUser.UserId, CurrentUser.ServiceUser.MobileServiceAuthenticationToken);
                        vault.Add(credential);
                    }
                    catch (InvalidOperationException)
                    {
                        await App.Current.NavigationService.Dispatcher.DispatchAsync(async () =>
                        {
                            MessageDialog dialog = new MessageDialog("Sign-in was canceled by the user.", "Whoops!");
                            await dialog.ShowAsync();
                        });
                    }



                }
                else
                {

                    CurrentUser.ServiceUser = new MobileServiceUser(credential.UserName);
                    credential.RetrievePassword();
                    CurrentUser.ServiceUser.MobileServiceAuthenticationToken = credential.Password;
                    
                      success = true;
                }
                
            }
            catch (TokenExpirationException)
            {
                await App.Current.NavigationService.Dispatcher.DispatchAsync(async () =>
                {
                    await new MessageDialog("An error occurred while we were determining the status of your authentication token.", "Whoops!").ShowAsync();
                });
            }
            catch (Exception ex)
            {
                await App.Current.NavigationService.Dispatcher.DispatchAsync(async () =>
                {
                    await new MessageDialog("An unknown error occurred during login: " + ex.Message, "Whoops!").ShowAsync();
                });
            }

            return await Task.FromResult(success);
        }
    }
}
