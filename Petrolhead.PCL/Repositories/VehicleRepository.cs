using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Petrolhead.Models;
using Petrolhead.ViewModels;
using System.Collections.ObjectModel;

namespace Petrolhead.Repositories
{
    /// <summary>
    /// Provides a way of creating new vehicles.
    /// </summary>
    public class VehicleRepository
    {
        public VehicleViewModel CreateVehicle(string name = null, string description = null, string make = null, string manufacturer = null, string model = null, string modelIdentifier = null, string engineType = null, DateTimeOffset? warrant = null, DateTimeOffset? rego = null, DateTimeOffset? purchaseYear = null, DateTimeOffset? manufactureYear = null, IList<Expense> expenses = null, IList<Reminder> reminders = null, decimal? budgetMax = null)
        {
            
            return new Vehicle()
            {
                Id = Guid.NewGuid().ToString(),
                Name = name ?? "",
                Description = description ?? "",
                Make = make ?? "",
                Manufacturer = manufacturer ?? "",
                Model = model ?? "",
                ModelIdentifier = modelIdentifier ?? "",
                EngineType = engineType ?? "",
                NextWarrantDate = warrant ?? DateTime.Today,
                NextRegistrationDate = rego ?? DateTime.Today,
                BudgetMaximum = budgetMax ?? 20000,
                IsOverBudget = false,
                UserId = "",
                Total = 0,
                Expenses = new ObservableCollection<Expense>(expenses.AsEnumerable()),
                Reminders = new ObservableCollection<Reminder>(reminders.AsEnumerable()),
                YearOfManufacture = manufactureYear ?? DateTime.Today,
                YearOfPurchase = manufactureYear ?? DateTime.Today,
            };
        }
    }
}
