using Windows.UI.Xaml;
using System.Threading.Tasks;
using Petrolhead.Services.SettingsServices;
using Windows.ApplicationModel.Activation;
using Template10.Controls;
using Template10.Common;
using System;
using System.Linq;
using Windows.UI.Xaml.Data;
using Microsoft.WindowsAzure.MobileServices;
using System.Collections.Generic;
using Windows.Security.Credentials;
using Windows.UI.Popups;
using System.Runtime.InteropServices;
using Petrolhead.ViewModels;

namespace Petrolhead
{
    /// Documentation on APIs used in this page:
    /// https://github.com/Windows-XAML/Template10/wiki

    [Bindable]
    sealed partial class App : Template10.Common.BootStrapper, IAuthenticator, IDialogHelper
    {
        #region App constructor
        public App()
        {
            // initializes XAML components
            InitializeComponent();
            // creates a new Splash object
            SplashFactory = (e) => new Views.Splash(e);
            // create some initial settings for Petrolhead
            #region App settings

            var _settings = SettingsService.Instance;
            RequestedTheme = _settings.AppTheme;
            CacheMaxDuration = _settings.CacheMaxDuration;
            ShowShellBackButton = _settings.UseShellBackButton;

            #endregion
        }
        #endregion

        #region Authentication helper methods
        public bool IsAuthenticated
        {
            get
            {
                return (User != null);
            }
        }

        public MobileServiceUser User
        {
            get; set;
        }

        public async Task<bool> AuthenticateAsync()
        {
            
            bool success = false;

            // generates credential storage objects
            PasswordVault vault = new PasswordVault();
            PasswordCredential credential = null;

            // sets default authentication provider (Microsoft account)
            MobileServiceAuthenticationProvider provider = MobileServiceAuthenticationProvider.MicrosoftAccount;
            var providerStr = provider.ToString();

            try
            {
                // Attempts to locate existing credentials in the vault
                credential = vault.FindAllByResource(providerStr).FirstOrDefault();
            }
            catch (Exception)
            {
                // Ignore errors in this portion of code
                // They have no effect on subsequent operations.
            }

            // Creates a DataStore object
            // Which provides the MobileServiceClient object
            // which is required for authentication purposes.
            DataStore store = await DataStore.Current();

            
            if (credential != null)
            {
                // An existing credential is present
                // In this situation, Petrolhead uses the existing credentials
                // Rather than attempting to reauthenticate.

                User = new MobileServiceUser(credential.UserName);
                credential.RetrievePassword();
                User.MobileServiceAuthenticationToken = credential.Password;

                store.CloudService.CurrentUser = User;

                // This code checks whether the current auth token is expired
                // And reauthenticates if so.
                if (store.CloudService.IsTokenExpired())
                {
                    try
                    {
                        User = await store.CloudService.LoginAsync(provider);
                        credential = new PasswordCredential(providerStr, User.UserId, User.MobileServiceAuthenticationToken);
                        vault.Add(credential);
                        success = true;
                    }
                    catch (Exception ex)
                    {

                    } 
                }
                else
                {
                    success = true;
                }
            }
            else
            {
                try
                {
                    // Authenticates with the server
                    User = await store.CloudService.LoginAsync(provider);
                    credential = new PasswordCredential(providerStr, User.UserId, User.MobileServiceAuthenticationToken);
                    vault.Add(credential);
                    success = true;
                }
                catch (Exception ex)
                {

                }
            }
            return success;
        }
        #endregion

        #region Launch methods
        public override async Task OnInitializeAsync(IActivatedEventArgs args)
        {
            if (Window.Current.Content as ModalDialog == null)
            {
                // create a new frame 
                var nav = NavigationServiceFactory(BackButton.Attach, ExistingContent.Include);

                // create modal root
                Window.Current.Content = new ModalDialog
                {
                    DisableBackButtonWhenModal = true,
                    Content = new Views.Shell(nav),
                    ModalContent = new Views.Busy(),
                };
            }
            await Task.CompletedTask;
        }

        public override async Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            
            // long-running startup tasks go here
            await Task.Delay(5000);

            NavigationService.Navigate(typeof(Views.MainPage));
            await Task.CompletedTask;
        }
        #endregion

        #region Dialog helper methods
        public async void ShowDialog(string content)
        {
            

            try
            {
                // The dialog is created on the UI thread, which blocks all other operations.
                await NavigationService.Dispatcher.DispatchAsync(async () =>
                {
                    try
                    {
                        // creates and displays a new dialog, with content and no title.
                        await new MessageDialog(content).ShowAsync();
                        
                    }
                    catch (COMException)
                    {

                    }
                    catch (Exception)
                    {

                    }
                });
            }
            catch (Exception)
            {

            }
            
        }

        public async void ShowDialog(string content, string title)
        {
            

            try
            {
                // The dialog is created on the UI thread, which blocks all other operations.
                await NavigationService.Dispatcher.DispatchAsync(async () =>
                {
                    try
                    {
                        // creates and displays a new dialog, with content and a title.
                        await new MessageDialog(content).ShowAsync();
                        
                    }
                    catch (COMException)
                    {

                    }
                    catch (Exception)
                    {

                    }
                });
            }
            catch (Exception)
            {

            }
            
        }

        #endregion


       


    }
}

