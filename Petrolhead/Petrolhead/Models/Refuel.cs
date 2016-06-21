using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petrolhead.Models
{
    /// <summary>
    /// Data model for a refuel
    /// </summary>
    public class Refuel : Expense
    {
        private FuelStation _station = default(FuelStation);
        public FuelStation Location { get { return _station; } set { Set(ref _station, value); } }
    }
}
