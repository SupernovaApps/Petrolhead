using Petrolhead.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(AuthenticationService))]
namespace Petrolhead.Services
{

    public class AuthenticationService : IAuthenticationService
    {
        readonly IAuthenticator _Authenticator;

       

        public AuthenticationService()
        {
            _Authenticator = DependencyService.Get<IAuthenticator>();
        }

        bool IsLoggedIn = true;

        public bool IsAuthenticated
        {
            get
            {
                return IsLoggedIn;
            }
        }

        public async Task<string> GetAuthenticationTokenAsync()
        {
            return await _Authenticator.GetAuthenticationTokenAsync();
        }

        public async Task<bool> LoginAsync()
        {
            IsLoggedIn = await _Authenticator.AuthenticateAsync();
            return true;
        }

        public async Task<bool> LogoutAsync()
        {
            await Task.Factory.StartNew(async () =>
            {
                var success = await _Authenticator.DeauthenticateAsync();

                if (!success)
                {
                    throw new Exception("Couldn't sign out!");
                }
                else
                    IsLoggedIn = false;
            });
            return true;
        }
    }
}
