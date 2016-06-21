using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petrolhead.Models
{
    /// <summary>
    /// Data model for a repair
    /// </summary>
    public sealed class Repair : Expense
    {
        private ObservableCollection<Component> _components = new ObservableCollection<Component>();
        public ObservableCollection<Component> Components
        {
            get
            {
                return _components;
            }
            set
            {
                Set(ref _components, value);


            }
        }


    }
}
