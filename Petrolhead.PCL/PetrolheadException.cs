using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petrolhead
{
    /// <summary>
    /// Base class for all Petrolhead exceptions
    /// </summary>
    /// <remarks>
    /// Not intended for use - just inherit from it.
    /// </remarks>
    public class PetrolheadException : Exception
    {
        public PetrolheadException(string message, Exception innerException) : base(message, innerException)
        {

        }

        public PetrolheadException(Exception innerException) : this("An error occurred.", innerException)
        {

        }

        public PetrolheadException(string message) : base(message)
        {

        }

        public PetrolheadException() : this("An error occurred")
        {

        }
    }
}
