using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petrolhead.Models
{
    /// <summary>
    /// Data model representing an expense
    ///</summary>
    public class Expense : ObservableObject
    {
        
       

        private string _id = Guid.NewGuid().ToString();
        public string Id { get { return _id; } set { Set(ref _id, value); } }

        private string _name = default(string);
        public string Name { get { return _name; } set { Set(ref _name, value); } }

        private string _description = default(string);
        public string Description { get { return _description; } set { Set(ref _description, value); } }

        protected double _cost = default(double);
        public virtual double Cost { get { return _cost; } set { Set(ref _cost, value);
                HumanCost = value.ToString("C");
            } }

        private string _humanCost = 0.00.ToString("C");
        public string HumanCost { get { return _humanCost; } set { Set(ref _humanCost, value); } }

        private long _mileage = default(long);
        public long Mileage { get { return _mileage; } set { Set(ref _mileage, value); } }

        private DateTimeOffset? _transactionDate = default(DateTimeOffset?);
        public DateTimeOffset? TransactionDate
        {
            get
            {
                return _transactionDate;
            }
            set
            {
                Set(ref _transactionDate, value);

                if (!value.HasValue)
                    HumanTransactionDate = 0.ToString("C");
                else
                    HumanTransactionDate = value.Value.ToString("C");
            }
        }

        private string _humanTransactionDate = default(string);
        public string HumanTransactionDate { get { return _humanTransactionDate; } private set { Set(ref _humanTransactionDate, value); } }
    }
}
