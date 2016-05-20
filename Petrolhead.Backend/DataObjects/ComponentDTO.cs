using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Petrolhead.Backend.DataObjects
{
    public class ComponentDTO : EntityData
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTimeOffset? DateRepaired { get; set; }
        public double Cost { get; set; }
        public string HumanCost { get; set; }
    }
}