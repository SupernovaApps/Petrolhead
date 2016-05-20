using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petrolhead.Models
{
    /// <summary>
    /// Data model representing a vehicle component
    /// </summary>
    public sealed class Component : ObservableObject
    {
        private string _id = default(string);
        public string Id { get { return _id; } set { Set(ref _id, value); } }

        

        private string _name = default(string);
        public string Name { get { return _name; } set { Set(ref _name, value); } }

        private string _description = default(string);
        public string Description { get { return _description; } set { Set(ref _description, value); } }

        private DateTimeOffset? _dateRepaired = default(DateTimeOffset?);
        public DateTimeOffset? DateRepaired { get { return _dateRepaired; } set { Set(ref _dateRepaired, value); } }

        
        

        private double _cost = default(double);
        public double Cost { get { return _cost; } set { Set(ref _cost, value); HumanCost = value.ToString("C"); } }

        private string _humanCost = default(string);
        public string HumanCost { get { return _humanCost; } private set { Set(ref _humanCost, value); } }


    }
}
