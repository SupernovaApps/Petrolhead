using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petrolhead.Models
{
    public sealed class Refuel : Expense
    {
        private FuelStation _station = default(FuelStation);
        public FuelStation Location { get { return _station; } set { Set(ref _station, value); } }
    }
}
