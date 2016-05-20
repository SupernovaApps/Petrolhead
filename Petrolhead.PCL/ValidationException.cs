using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petrolhead
{
    public sealed class ValidationException : CorePCLException
    {
        public ValidationException() : this("Validation error", null)
        {

        }
        public ValidationException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
