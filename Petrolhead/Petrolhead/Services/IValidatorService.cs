using Petrolhead.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petrolhead.Services
{
    public interface IValidatorService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="vehicle">Vehicle to be validated</param>
        /// <returns>Boolean (asynchronous)</returns>
        Task<bool> ValidateVehicleAsync(Vehicle vehicle);
        Task<bool> ValidateExpenseAsync(Expense expense);
        Task<bool> ValidateRepairAsync(Repair repair);
        Task<bool> ValidateComponentAsync(Component component);
        Task<bool> ValidateRefuelAsync(Refuel refuel);
        Task<bool> ValidateFuelStationAsync(FuelStation station);
    }
}
