using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Petrolhead.Backend.DataObjects
{
    public class RepairDTO : ExpenseDTO
    {
        public List<ComponentDTO> Components { get; set; }
    }
}