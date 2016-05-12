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
    /// Client data model for vehicle objects
    /// </summary>
    public class Vehicle : ObservableObject
    {
        internal Vehicle()
        {

        }


        #region UUIDs
        private string _userId = default(string);
        public string UserId { get { return _userId; } set { Set(ref _userId, value); } }

        private string _id = default(string);
        public string Id { get { return _id; } set { Set(ref _id, value); } }
        #endregion

        #region Strings
        private string _name = default(string);
        public string Name { get { return _name; } set { Set(ref _name, value); } }

        private string _description = default(string);
        public string Description { get { return _description; } set { Set(ref _description, value); } }

        private string _make = default(string);
        public string Make { get { return _make; } set { Set(ref _make, value); } }

        private string _manufacturer = default(string);
        public string Manufacturer { get { return _manufacturer; } set { Set(ref _manufacturer, value); } }

        private string _model = default(string);
        public string Model { get { return _model; } set { Set(ref _model, value); } }

        private string _modelIdentifier = default(string);
        public string ModelIdentifier { get { return _modelIdentifier; } set { Set(ref _modelIdentifier, value); } }

        private string _engineType = default(string);
        public string EngineType { get { return _engineType; } set { Set(ref _engineType, value); } }
        #endregion


        #region Reminder dates
        private DateTimeOffset? _nextWarrantDate = default(DateTimeOffset?);
        public DateTimeOffset? NextWarrantDate { get { return _nextWarrantDate; } set { Set(ref _nextWarrantDate, value); } }

        

        private DateTimeOffset? _nextRegoDate = default(DateTimeOffset?);
        public DateTimeOffset? NextRegistrationDate { get { return _nextRegoDate; } set { Set(ref _nextRegoDate, value); } }

        private DateTimeOffset? _yearOfPurchase = default(DateTimeOffset?);
        public DateTimeOffset? YearOfPurchase { get { return _yearOfPurchase; } set { Set(ref _yearOfPurchase, value); } }

    


        private DateTimeOffset? _yearOfManufacture = default(DateTimeOffset?);
        public DateTimeOffset? YearOfManufacture { get { return _yearOfManufacture; } set { Set(ref _yearOfManufacture, value); } }

       


        #endregion

        #region Totals
        private decimal _total = default(decimal);
        public decimal Total { get { return _total; } set { Set(ref _total, value); } }

        private decimal _budgetMax = default(decimal);
        public decimal BudgetMaximum { get { return _budgetMax; } set { Set(ref _budgetMax, value); } }

        private bool _isOverBudget = false;
        public bool IsOverBudget { get { return _isOverBudget; } set
            {
                Set(ref _isOverBudget, value);
            } }
        #endregion

        #region Components
        private ObservableCollection<Expense> _expenses = new ObservableCollection<Expense>();
        public ObservableCollection<Expense> Expenses { get
            {
                return _expenses;
            }
            set
            {
                Set(ref _expenses, value);
            }
        }

        private ObservableCollection<Reminder> _reminders = new ObservableCollection<Reminder>();
        public ObservableCollection<Reminder> Reminders
        {
            get
            {
                return _reminders;
            }
            set
            {
                Set(ref _reminders, value);
            }
        }
        #endregion






    }
}
