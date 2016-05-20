using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Petrolhead.Backend.DataObjects
{
    public class Repair : Expense
    {
        public IList<Component> Components { get; set; }
    }
}