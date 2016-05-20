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
using Windows.Security.Credentials;
using System.Diagnostics;
using System.Runtime.InteropServices;
using NotificationsExtensions.Toasts;

namespace Petrolhead
{
    /// Documentation on APIs used in this page:
    /// https://github.com/Windows-XAML/Template10/wiki

    [Bindable]
    sealed partial class App : Template10.Common.BootStrapper, IAuthenticator
    {
        public App()
        {
           
            InitializeComponent();
            SplashFactory = (e) => new Views.Splash(e);

            #region App settings

            var _settings = SettingsService.Instance;
            RequestedTheme = _settings.AppTheme;
            CacheMaxDuration = _settings.CacheMaxDuration;
            ShowShellBackButton = _settings.UseShellBackButton;

            #endregion
        }

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

            try
            {
                PasswordVault vault = new PasswordVault();
                PasswordCredential credential = null;
                var store = await DataStore.Current();

                try
                {
                    credential = vault.FindAllByResource(MobileServiceAuthenticationProvider.MicrosoftAccount.ToString()).FirstOrDefault();
                }
                catch (COMException)
                {
                    Debug.WriteLine("Ignoring error");
                }
                catch (Exception)
                {
                    Debug.WriteLine("Ignoring error");
                }

                if (credential != null)
                {
                    store.CloudService.CurrentUser = new MobileServiceUser(credential.UserName);
                    credential.RetrievePassword();
                    store.CloudService.CurrentUser.MobileServiceAuthenticationToken = credential.Password;

                    if (store.CloudService.IsTokenExpired())
                    {
                        User = await store.CloudService.LoginAsync(MobileServiceAuthenticationProvider.MicrosoftAccount);
                    }
                    else
                    {
                        User = store.CloudService.CurrentUser;
                    }
                }
                else
                {
                    User = await store.CloudService.LoginAsync(MobileServiceAuthenticationProvider.MicrosoftAccount);
                }

                ToastContent content = new ToastContent()
                {

                };
            }
            catch (InvalidOperationException)
            {

            }
            catch (Exception ex)
            {

            }

            return success;
        }

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
            CoreApp.Initialize(DialogHelper.Current, this);
            await Task.Delay(1000);

            NavigationService.Navigate(typeof(Views.MainPage));
            await Task.CompletedTask;
        }
    }
}

