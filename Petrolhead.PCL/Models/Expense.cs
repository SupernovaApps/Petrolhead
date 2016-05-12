using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petrolhead.Models
{
    /// <summary>
    /// Client data model for expense objects
    /// </summary>
    public class Expense : ObservableObject
    {
        internal Expense()
        {

        }

        public override string ToString()
        {
            return "";
        }

        public enum ExpenseType
        {
            Generic,
            Maintenance,
            Fuel
        }

        private string _id = default(string);
        public string Id { get { return _id; } set { Set(ref _id, value); } }

        private string _name = default(string);
        public string Name { get { return _name; } set { Set(ref _name, value); } }

        private string _description = default(string);
        public string Description { get { return _description; } set { Set(ref _description, value); } }

        private decimal _cost = default(decimal);
        public decimal Cost { get { return _cost; } set { Set(ref _cost, value);
                HumanCost = value.ToString("C");
            } }

        private ExpenseType _type = default(ExpenseType);
        public ExpenseType Type { get { return _type; } set { Set(ref _type, value); HumanType = value.ToString(); } }

        private string _humanType = default(string);
        public string HumanType { get { return _humanType; } private set { Set(ref _humanType, value); } }

        private string _humanCost = default(string);
        public string HumanCost { get { return _humanCost; } private set { Set(ref _humanCost, value); } }

        private decimal _mileage


    }
}
