using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petrolhead.Data
{
    /// <summary>
    /// Status of the application login
    /// </summary>
    public enum LoginStatus
    {
        Success,
        Canceled,
        NetworkError,
        UnknownError
    }

    /// <summary>
    /// Exception thrown when an error occurs during login.
    /// </summary>
    public class LoginFailureException : PetrolheadException
    {
        public LoginStatus Status { get; private set; }

        public LoginFailureException(string message, LoginStatus loginStatus, Exception innerException) : base(message, innerException)
        {
            Status = loginStatus;
        }
    }
}
