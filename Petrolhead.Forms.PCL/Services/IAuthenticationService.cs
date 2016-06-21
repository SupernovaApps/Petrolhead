using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petrolhead.Services
{
    /// <summary>
    /// Provides services for authentication
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        /// Logs the user into the server.
        /// </summary>
        /// <returns>Boolean (asynchronous)</returns>
        Task<bool> LoginAsync();
        /// <summary>
        /// Logs the user out of the server.
        /// </summary>
        /// <returns>Boolean (asynchronous)</returns>
        Task<bool> LogoutAsync();
        /// <summary>
        /// Gets the users authentication token
        /// </summary>
        /// <returns>String (asynchronous)</returns>
        Task<string> GetAuthenticationTokenAsync();
        bool IsAuthenticated { get; }
    }
}
