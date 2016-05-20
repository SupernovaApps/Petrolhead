using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petrolhead
{
    public class CorePCLException : Exception
    {
        public CorePCLException(string message, Exception innerException) : base (message, innerException)
        {

        }

        public CorePCLException(string message) : this(message, null)
        {

        }

        public CorePCLException() : this("An error occurred in Petrolhead's PCL")
        {

        }
    }
}
