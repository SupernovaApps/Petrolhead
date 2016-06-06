using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petrolhead.Models
{
    /// <summary>
    /// Data model representing a vehicle
    /// </summary>
    public sealed class Vehicle : ObservableObject
    {

        public Vehicle()
        {
            Id = Guid.NewGuid().ToString();


        }

       
        

        private string _id = default(string);
        public string Id { get { return _id; } set { Set(ref _id, value); } }

        private string _name = default(string);
        public string Name { get { return _name; } set { Set(ref _name, value); } }

        private string _description = default(string);
        public string Description { get { return _description; } set { Set(ref _description, value); } }

        private string _manufacturer = default(string);
        public string Manufacturer { get { return _manufacturer; } set { Set(ref _manufacturer, value); } }

        private string _model = default(string);
        public string Model { get { return _model; } set { Set(ref _model, value); } }

        private string _modelId = default(string);
        public string ModelIdentifier { get { return _modelId; } set { Set(ref _modelId, value); } }

        private double _total = default(double);
        public double Total { get { return _total; } set { Set(ref _total, value); HumanTotal = value.ToString("C"); } }

        private string _humanTotal = default(string);
        public string HumanTotal { get { return _humanTotal; } private set { Set(ref _humanTotal, value); } }

        

        private DateTimeOffset? _nextWarrantDate = default(DateTimeOffset?);
        public DateTimeOffset? NextWarrantDate { get { return _nextWarrantDate; } set { Set(ref _nextWarrantDate, value); } }

        private DateTimeOffset? _nextRegoRenewal = default(DateTimeOffset?);
        public DateTimeOffset? NextRegoRenewal { get { return _nextRegoRenewal; } set { Set(ref _nextRegoRenewal, value); } }

        private DateTimeOffset? _yearOfPurchase = default(DateTimeOffset?);
        public DateTimeOffset? YearOfPurchase { get { return _yearOfPurchase; } set { Set(ref _yearOfPurchase, value); } }

        private DateTimeOffset? _yearOfManufacture = default(DateTimeOffset?);
        public DateTimeOffset? YearOfManufacture { get { return _yearOfManufacture; } set { Set(ref _yearOfManufacture, value); } }

        


        private ObservableCollection<Expense> _expenses = new ObservableCollection<Expense>();
        public ObservableCollection<Expense> Expenses
        {
            get
            {
                return _expenses;
            }
            set
            {
                Set(ref _expenses, value);

               
                
         
                    

            }
        }

        private Expense _selectedExpense = default(Expense);
        public Expense SelectedExpense { get { return _selectedExpense; } set { Set(ref _selectedExpense, value); } }


        private bool _isOverBudget = default(bool);
        public bool IsOverBudget
        {
            get
            {
                return _isOverBudget;
            }
            set
            {
                Set(ref _isOverBudget, value);
            }
        }

        private double _budgetMax = default(double);
        public double BudgetMax { get
            {
                return _budgetMax;
            }
            set
            {
                Set(ref _budgetMax, value);
            }
        }
        private ObservableCollection<Repair> _repairs = new ObservableCollection<Repair>();
        public ObservableCollection<Repair> Repairs
        {
            get
            {
                return _repairs;
            }
            set
            {
                Set(ref _repairs, value);
                

            }
        }

        private Repair _selectedRepair = default(Repair);
        public Repair SelectedRepair { get { return _selectedRepair; } set { Set(ref _selectedRepair, value); } }

        private ObservableCollection<Refuel> _refuels = new ObservableCollection<Refuel>();
        public ObservableCollection<Refuel> Refuels
        {
            get
            {
                return _refuels;
            }
            set
            {
                Set(ref _refuels, value);
                
                
            }
        }

        private Refuel _selectedRefuel = default(Refuel);
        public Refuel SelectedRefuel { get { return _selectedRefuel; } set { Set(ref _selectedRefuel, value); } }
    }
}
