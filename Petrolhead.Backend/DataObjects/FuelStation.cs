﻿using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Petrolhead.Backend.DataObjects
{
    public class FuelStation : EntityData
    {
        public string Location { get; set; }
        public Refuel Refuel { get; set; }
    }
}