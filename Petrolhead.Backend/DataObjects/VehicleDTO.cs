using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Petrolhead.Backend.DataObjects
{
    public class VehicleDTO : EntityData
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
        public List<ExpenseDTO> Expenses { get; set; }
        public ExpenseDTO SelectedExpense { get; set; }
        public List<RefuelDTO> Refuels { get; set; }
        public RefuelDTO SelectedRefuel { get; set; }
        public List<RepairDTO> Repairs { get; set; }
        public RepairDTO SelectedRepair { get; set; }

    }
}