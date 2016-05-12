using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace Petrolhead.Models
{
    /// <summary>
    /// Client data model for reminder objects
    /// </summary>
    public class Reminder : ObservableObject
    {
        internal Reminder()
        {

        }

        private string _id = default(string);
        public string Id
        {
            get
            {
                return _id;
            }
            set
            {
                Set(ref _id, value);
            }
        }
    }
}
