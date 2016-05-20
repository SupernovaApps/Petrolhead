using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Windows.Security.Credentials;

namespace Petrolhead.UWP.BackgroundTasks
{
    internal class Authenticator : IAuthenticator
    {
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
            PasswordVault vault = new PasswordVault();
            PasswordCredential credential = null;

            try
            {
                credential = vault.FindAllByResource(MobileServiceAuthenticationProvider.MicrosoftAccount.ToString()).FirstOrDefault();
            }
            catch (Exception)
            {

            }

            DataStore store = await DataStore.Current();
            try
            {
                if (credential != null)
                {
                    User = new MobileServiceUser(credential.UserName);
                    credential.RetrievePassword();
                    User.MobileServiceAuthenticationToken = credential.Password;
                    store.CloudService.CurrentUser = User;

                    if (store.CloudService.IsTokenExpired())
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
