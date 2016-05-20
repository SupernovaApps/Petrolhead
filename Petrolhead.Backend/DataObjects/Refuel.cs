using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Petrolhead.Backend.DataObjects
{
    public class Refuel : Expense
    {
        public FuelStation Location { get; set; }
    }
}