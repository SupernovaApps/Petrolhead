using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Petrolhead.Backend.DataObjects
{
    public class RefuelDTO : ExpenseDTO
    {
        public FuelStationDTO Location { get; set; }
    }
}