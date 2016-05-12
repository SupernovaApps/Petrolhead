using Microsoft.WindowsAzure.MobileServices;
using Petrolhead.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petrolhead.Data
{
    public interface IAuthenticator
    {
        Task<bool> AuthenticateAsync();
        User CurrentUser { get; set; }
    }
}
