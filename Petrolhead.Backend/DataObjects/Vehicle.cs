using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Petrolhead.Backend.DataObjects
{
    public class Vehicle : EntityData
    {
        public string UserID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string ModelIdentifier { get; set; }
        public bool IsOverBudget { get; set; }
        public double BudgetMax { get; set; }
        public double Total { get; set; }
        public string HumanTotal { get; set; }
        public DateTimeOffset? NextWarrantDate { get; set; }
        public DateTimeOffset? NextRegoRenewal { get; set; }
        public DateTimeOffset? YearOfPurchase { get; set; }
        public DateTimeOffset? YearOfManufacture { get; set; }
        public IList<Expense> Expenses { get; set; }
        public Expense SelectedExpense { get; set; }
        public IList<Repair> Repairs { get; set; }
        public Repair SelectedRepair { get; set; }
        public IList<Refuel> Refuels { get; set; }
        public Refuel SelectedRefuel { get; set; }
        
    }
}