using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petrolhead
{
    public sealed class DialogHelperException : PetrolheadUserInterfaceException
    {
        public DialogHelperException(string componentName, Exception innerException) : base(innerException.Message, componentName, innerException)
        {

        }
    }
}
