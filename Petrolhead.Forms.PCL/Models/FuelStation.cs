using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petrolhead.Models
{
    public class FuelStation : ObservableObject
    {
        private string _id = default(string);
        public string Id { get { return _id; } set { Set(ref _id, value); } }

        private string _location = default(string);
        public string Location { get { return _location; } set { Set(ref _location, value); } }
    }
}
