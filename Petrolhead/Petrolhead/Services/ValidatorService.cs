using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Petrolhead.Models;

namespace Petrolhead.Services
{
    public sealed class ValidatorService : IValidatorService
    {
        public async Task<bool> ValidateComponentAsync(Component component)
        {
            bool success = false;

            return success;
        }

        public async Task<bool> ValidateExpenseAsync(Expense expense)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ValidateFuelStationAsync(FuelStation station)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ValidateRefuelAsync(Refuel refuel)
        {
            bool success = false;

            if (await ValidateExpenseAsync(refuel))
            {

            }
            return success;
        }

        public Task<bool> ValidateRepairAsync(Repair repair)
        {
            throw new NotImplementedException();
        }

        
        public async Task<bool> ValidateVehicleAsync(Vehicle vehicle)
        {
            bool success = false;

            if (string.IsNullOrWhiteSpace(vehicle.Name))
            {
                

            }

            return success;
        }

        private static IValidatorService _current = new ValidatorService();
        public static IValidatorService CurrentValidator
        {
            get
            {
                if (_current == null)
                    _current = new ValidatorService();
                return _current;
            }
        }

        private ValidatorService()
        {
            _current = this;
        }
    }
}
