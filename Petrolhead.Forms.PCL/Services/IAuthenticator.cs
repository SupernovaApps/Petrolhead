using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petrolhead.Services
{
    /// <summary>
    /// Provides methods for authenticating the user with the server.
    /// </summary>
    public interface IAuthenticator
    {
        /// <summary>
        /// Authenticates the user.
        /// </summary>
        /// <returns>Boolean (asynchronous)</returns>
        Task<bool> AuthenticateAsync();
        /// <summary>
        /// Deauthenticates the user.
        /// </summary>
        /// <returns>Boolean (asynchronous)</returns>
        Task<bool> DeauthenticateAsync();
        /// <summary>
        /// Gets the current authentication token
        /// </summary>
        /// <returns>String (asynchronous)</returns>
        Task<string> GetAuthenticationTokenAsync();
    }
}
