using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petrolhead.Services
{
    public interface IAuthenticator
    {
        Task<bool> LoginAsync();
        Task<bool> LogoutAsync();
    }
}
