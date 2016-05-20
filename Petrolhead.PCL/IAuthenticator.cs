using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petrolhead
{
    public interface IAuthenticator
    {
        bool IsAuthenticated { get; }
        MobileServiceUser User { get; set; }
        Task<bool> AuthenticateAsync();
    }
}
