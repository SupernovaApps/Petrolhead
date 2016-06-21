using Petrolhead.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petrolhead.Services
{
    public interface IDataStore
    {
        Task Init();
        IAuthenticator Authenticator { get; set; }
        Task<IEnumerable<Vehicle>> GetVehiclesAsync();
        Task<Vehicle> AddVehicleAsync(Vehicle vehicle);
        Task<bool> RemoveVehicleAsync(Vehicle vehicle);
        Task SyncVehiclesAsync();
    }
}
