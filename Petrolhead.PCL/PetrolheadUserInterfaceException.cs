using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petrolhead
{
    public class PetrolheadUserInterfaceException : CorePCLException
    {
        public string UIComponentName { get; private set; }

       
        public PetrolheadUserInterfaceException(string componentName) : this("A user interface-related error occurred", componentName)
        {
            
        }

        public PetrolheadUserInterfaceException(string message, string componentName) : this(message, componentName, null)
        {
            UIComponentName = componentName;
        }

        public PetrolheadUserInterfaceException(string message, string componentName, Exception innerException) : base(message, innerException)
        {
            UIComponentName = componentName;
        }
    }
}
